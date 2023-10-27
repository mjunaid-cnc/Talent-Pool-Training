using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Domain.Entities;
using Task5.Domain.Models;

namespace Task5.Domain.Mappers
{
    public class EmployeeMapper
    {
        public static Employee MapEmployee(AddEmployeeRequestModel addEmployeeRequest)
        {
            Employee employee = new Employee()
            {
                Name = addEmployeeRequest.Name,
                Salary = addEmployeeRequest.Salary,
                Email = addEmployeeRequest.Email,
            };
            return employee;
        }
    }
}
