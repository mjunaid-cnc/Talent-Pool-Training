using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Application.Interfaces.Repositories;
using Task5.Domain.Entities;
using Task5.Infrastructure.DB;

namespace Task5.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        Database db = new Database();
        public async Task<int> AddEmployee(Employee employee)
        {
            string query = "INSERT INTO Employees(Name,Salary,Email,UserId) VALUES(@Name,@Salary,@Email,@UserId)";
            var parameters = new IDataParameter[]
                {
                    new SqlParameter("@Name", employee.Name),
                    new SqlParameter("@Salary", employee.Salary),
                    new SqlParameter("@Email", employee.Email),
                    new SqlParameter("@UserId", employee.UserId)
                };
            int resultRows = db.ExecuteData(query, parameters);
            return resultRows;
        }

        public async Task<Employee?> GetEmployeeByEmail(string email)
        {
            string query = $"SELECT * FROM Employees WHERE Email = '{email}'";
            DataTable dt = db.GetTable(query);
            if (dt.Rows.Count > 0)
            {
                var employee = new Employee()
                {
                    Id = int.Parse(dt.Rows[0]["Id"].ToString()!),
                    Name = dt.Rows[0]["Name"].ToString()!,
                    Salary = double.Parse(dt.Rows[0]["Salary"].ToString()!),
                    Email = dt.Rows[0]["Email"].ToString()!,
                    UserId = int.Parse(dt.Rows[0]["UserId"].ToString()!)
                };
                return employee;
            }
            return null;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            try
            {
                string query = "SELECT * FROM Employees";
                DataTable dt = db.GetTable(query);
                List<Employee> employees = new List<Employee>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var employee = new Employee()
                    {
                        Id = int.Parse(dt.Rows[i]["Id"].ToString()!),
                        Name = dt.Rows[i]["Name"].ToString()!,
                        Salary = double.Parse(dt.Rows[i]["Salary"].ToString()!),
                        Email = dt.Rows[i]["Email"].ToString()!,
                        UserId = int.Parse(dt.Rows[i]["UserId"].ToString()!)
                    };
                    employees.Add(employee);
                }
                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Employee? GetEmployeeById(int id)
        {
            string query = $"SELECT * FROM Employees WHERE Id = {id}";
            DataTable dt = db.GetTable(query);
            if (dt.Rows.Count > 0)
            {
                var employee = new Employee()
                {
                    Id = int.Parse(dt.Rows[0]["Id"].ToString()!),
                    Name = dt.Rows[0]["Name"].ToString()!,
                    Salary = double.Parse(dt.Rows[0]["Salary"].ToString()!),
                    Email = dt.Rows[0]["Email"].ToString()!,
                    UserId = int.Parse(dt.Rows[0]["UserId"].ToString()!)
                };
                return employee;
            }
            return null;
        }
    }
}
