using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Models;

namespace Task5.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeRequestModel addEmployeeRequest)
        {
            try
            {
                var addEmployeeResult = await _employeeService.AddEmployee(addEmployeeRequest);
                if (!addEmployeeResult.Success)
                    return Ok(new Response { Success = false, Message = addEmployeeResult.Message, StatusCode = StatusCodes.Status400BadRequest });

                return Ok(new Response { Success = true, Message = "Employee added", StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var addEmployeeResult = await _employeeService.GetAllEmployees();
                return Ok(new Response { Success = true, Content = addEmployeeResult.Content, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }
    }
}
