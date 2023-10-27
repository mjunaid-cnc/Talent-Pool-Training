using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Task5.Application.Interfaces.Repositories;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Entities;
using Task5.Domain.Mappers;
using Task5.Domain.Models;

namespace Task5.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<Response> Register(RegisterRequestModel registerRequest)
        {
            try
            {
                if (string.Equals(registerRequest.Role, Domain.Helpers.Roles.User.ToString()))
                {
                    var user = await _userRepository.GetUserByEmail(registerRequest.Email);
                    if (user != null)
                    {
                        return new Response { Success = false, Message = "User already exists" };
                    }
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
                    var newUser = UserMapper.MapNewUser(registerRequest, hashedPassword);
                    int resultRows = await _userRepository.AddUser(newUser);
                    if (resultRows < 0)
                    {
                        throw new ApplicationException("Something went wrong");
                    }
                    return new Response { Success = true };
                }
                return new Response { Success = false, Message = "Unauthorized request" };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response> Login(LoginRequestModel loginRequestModel)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(loginRequestModel.Email);
                if (user == null)
                    return new Response { Success = false, Message = "Incorrect email" };
                bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(loginRequestModel.Password, user.Password);
                if (!isCorrectPassword)
                    return new Response { Success = false, Message = "Incorrect password" };
                
                var jwtSigningKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSigningKey), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new Response { Success = true, Content = tokenHandler.WriteToken(token) };
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
