using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Task7.API.Models;
using Amazon;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Task7.API.Repositories
{
    public class UserRepo
    {
        public async Task<List<EmployeeModel>> GetEmployees()
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
                List<EmployeeModel> employees = new();
                using (SqlConnection connection = new SqlConnection(connectionObj.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Employees";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.GetString(reader.GetOrdinal("Name"));
                                string salary = reader.GetString(reader.GetOrdinal("Salary"));
                                string email = reader.GetString(reader.GetOrdinal("Email"));

                                var employee = new EmployeeModel()
                                {
                                    Name = name,
                                    Salary = salary,
                                    Email = email,
                                };
                                employees.Add(employee);
                            }
                        }
                    }
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
