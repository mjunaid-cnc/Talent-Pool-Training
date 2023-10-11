using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Interfaces;
using Todo.Domain.Models;
using Todo.Domain.Models.Authentication;

namespace Task2_BasicWebApiCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService) 
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                var result = await _authenticationService.Register(model);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return Ok(new Response { Message = string.Join(",", errors), StatusCode = StatusCodes.Status400BadRequest });
                }
                return Ok(new Response { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message=ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    try
        //    {
        //        var response = 
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new Response { Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
        //    }
        //}
    }
}
