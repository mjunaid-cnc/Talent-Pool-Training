using Amazon;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Task7SQSLambda.Models;

namespace Task7SQSLambda.Repositories
{
    public class UserRepo
    {
        public async Task AddEmployee(EmployeeModel employee)
        {
            try
            {
                var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName("eu-north-1"));
                var request = new GetSecretValueRequest()
                {
                    SecretId = "task7Secret",
                };
                var response = await client.GetSecretValueAsync(request);
                var connectionObj = JsonSerializer.Deserialize<ConnectionStringModel>(response.SecretString) ?? throw new Exception("Secret is incorrect");
                
                using (SqlConnection connection = new SqlConnection(connectionObj.ConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Employees(Name, Email, Salary) VALUES (@Name, @Email, @Salary)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", employee.Name);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@Salary", employee.Salary);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
