using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Application.Interfaces.Repositories;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Mappers;
using Task5.Domain.Models;

namespace Task5.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<Response> Register(RegisterRequestModel registerRequest)
        {
            try
            {
                if (string.Equals(registerRequest.Role, Domain.Helpers.Roles.User))
                var userCount = await _userRepository.GetUserByEmail(registerRequest.Email);
                if (userCount != 0)
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> Login(LoginRequestModel loginRequestModel)
        {
            return "Login";
        }

    }
}
