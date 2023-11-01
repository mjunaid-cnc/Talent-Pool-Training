using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Task5.ActionFilters;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Entities;
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
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var getEmployeesResult = await _employeeService.GetAllEmployees();
                return Ok(new Response { Success = true, Content = getEmployeesResult.Content, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [ServiceFilter(typeof(CheckUserAccessActionFilter))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var getEmployeeResult = await _employeeService.GetEmployeeById(id);
                return Ok(new Response { Success = true, Content = getEmployeeResult.Content, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [HttpPost("delete-or-update")]
        public IActionResult DeleteAndUpdateEmployee(AddEmployeeRequestModel employeeRequestModel)
        {
            try
            {
                var deleteOrEditResult = _employeeService.DeleteOrEditEmployee(employeeRequestModel);
                if (!deleteOrEditResult.Success)
                    return Ok(new Response { Success = false, Message = "Something went wrong", StatusCode = StatusCodes.Status500InternalServerError });
                return Ok(new Response { Success = true, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [HttpPost("upload-excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            try
            {
                var uploadExcelResult = await _employeeService.UploadBulkData(file);
                if (!uploadExcelResult.Success)
                    return Ok(new Response { Success = false, StatusCode = StatusCodes.Status400BadRequest });
                return Ok(new Response { Success = true, StatusCode= StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }
    }
}
