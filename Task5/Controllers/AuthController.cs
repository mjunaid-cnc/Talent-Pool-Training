
using Microsoft.AspNetCore.Mvc;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Models;

namespace Task5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                var registrationResult = await _authService.Register(registerRequest);
                if (!registrationResult.Success)
                {
                    return Ok(new Response { Success = false, Message = registrationResult.Message, StatusCode = StatusCodes.Status400BadRequest });
                }
                return Ok(new Response { Success = true, Message = "Registration successful", StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }
    }
}
