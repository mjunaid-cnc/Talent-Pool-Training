using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interfaces;
using Todo.Domain.Models.Authentication;

namespace Todo.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticationService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
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
                IdentityUser user = new()
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

        //public async Task<string> Authenticate(LoginModel model)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}

    }
}
