using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Domain.Models;

namespace Task5.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<Response> AddEmployee(AddEmployeeRequestModel addEmployeeRequest);
        Task<Response> GetAllEmployees();
    }
}
