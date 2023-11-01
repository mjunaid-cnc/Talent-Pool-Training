using Azure;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Application.Interfaces.Repositories;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Entities;
using Task5.Domain.Mappers;
using Task5.Domain.Models;
using Response = Task5.Domain.Models.Response;

namespace Task5.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserResolverService _userResolver;
        public EmployeeService(IHttpContextAccessor contextAccessor, IEmployeeRepository employeeRepository, IUserResolverService userResolver)
        {
            _contextAccessor = contextAccessor;
            _employeeRepository = employeeRepository;
            _userResolver = userResolver;
        }

        public async Task<Response> AddEmployee(AddEmployeeRequestModel addEmployeeRequest)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByEmail(addEmployeeRequest.Email);
                if (employee != null)
                {
                    return new Response { Success = false, Message = "Email already exists" };
                }
                var employeeToAdd = EmployeeMapper.MapEmployee(addEmployeeRequest);
                employeeToAdd.UserId = int.Parse(_contextAccessor.HttpContext.User.FindFirst("id")!.Value);
                var resultRows = await _employeeRepository.AddEmployee(employeeToAdd);
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

        public async Task<Response> GetAllEmployees()
        {
            try
            {
                int userId = _userResolver.GetUserId();
                string role = _userResolver.GetUserRole();
                var employees = await _employeeRepository.GetEmployees();
                if (string.Equals(role, Domain.Helpers.Roles.User.ToString()))
                    employees = employees.Where(x => x.UserId == userId).ToList();
                return new Response { Content = employees };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response> GetEmployeeById(int id)
        {
            try
            {
                int userId = _userResolver.GetUserId();
                string role = _userResolver.GetUserRole();
                var employee = _employeeRepository.GetEmployeeById(id);
                return new Response { Content = employee };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Response DeleteOrEditEmployee(AddEmployeeRequestModel employeeRequestModel)
        {
            try
            {
                var existingEmployee = _employeeRepository.GetEmployeeByEmail(employeeRequestModel.Email).Result;
                if (existingEmployee != null && existingEmployee.Email == employeeRequestModel.Email && existingEmployee.Id != employeeRequestModel.EmployeeId)
                    return new Response { Success = false, Message = "Email already exists" };
                bool successFlag = _employeeRepository.DeleteOrUpdateSP(employeeRequestModel);
                if (!successFlag)
                {
                    return new Response { Success = false, Message = "Something went wrong" };
                }
                return new Response { Success = true };

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response> UploadBulkData(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
              !file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        return new Response { Success = false, Message = "Invalid file type. Please upload an Excel file (.xlsx)." };
                    }
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                            {
                                var newEmployee = new Employee
                                {
                                    Name = worksheet.Cells[row, 1].Value.ToString()!,
                                    Salary = double.Parse(worksheet.Cells[row, 2].Value.ToString()!),
                                    Email = worksheet.Cells[row, 3].Value.ToString()!
                                };
                                var employee = await _employeeRepository.GetEmployeeByEmail(newEmployee.Email);
                                if (employee != null)
                                {
                                    return new Response { Success = false, Message = "Email already exists" };
                                }
                                newEmployee.UserId = int.Parse(_contextAccessor.HttpContext.User.FindFirst("id")!.Value);
                                var resultRows = await _employeeRepository.AddEmployee(newEmployee);
                                if (resultRows < 0)
                                {
                                    throw new ApplicationException("Something went wrong");
                                }
                                return new Response { Success = true };
                            }
                        }
                    }
                }
                return new Response { Success = false, Message = "Please select a file to upload" };
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
