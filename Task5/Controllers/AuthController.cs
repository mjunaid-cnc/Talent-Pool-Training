
using Microsoft.AspNetCore.Mvc;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Models;

namespace Task5.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestModel registerRequest)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message, StatusCode = Stat };
    }
}
