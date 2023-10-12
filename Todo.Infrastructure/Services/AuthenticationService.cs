using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;
using Todo.Domain.Models.Authentication;

namespace Todo.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticationService(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> Register(RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null) return IdentityResult.Failed(new IdentityError { Description = "User already exists" });
                User user = new()
                {
                    UserName = model.Username,
                };
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        return IdentityResult.Failed(result.Errors.FirstOrDefault());
                    }
                    var addUserRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
                    return addUserRoleResult.Succeeded ? IdentityResult.Success
                        : IdentityResult.Failed(new IdentityError { Description = "Failed to register user" });
                }
                else
                {
                    throw new ApplicationException($"Role {model.Role} does not exist");
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description= ex.Message });
            }
        }

        public async Task<string> Authenticate(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username) ?? throw new ApplicationException("User does not exist");
                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isCorrectPassword) throw new ApplicationException("Incorrect password");
                var roles = await _userManager.GetRolesAsync(user);

                var jwtSigningKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, string.Join(",", roles))
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSigningKey), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
