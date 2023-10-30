using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Domain.Entities;

namespace Task5.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<int> AddEmployee(Employee employee);
        Task<Employee?> GetEmployeeByEmail(string email);
        Task<List<Employee>> GetEmployees();
        Employee? GetEmployeeById(int id);
    }
}
