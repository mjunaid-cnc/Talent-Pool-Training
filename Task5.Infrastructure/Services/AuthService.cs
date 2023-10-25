using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Application.Interfaces.Repositories;
using Task5.Application.Interfaces.Services;
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
                var user = await _userRepository.GetUserByEmail(registerRequest.Email);
                if (user != null)
                {
                    return new Response { Success = false, Message = "User already exists" };
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
