using Amazon.Lambda.Core;
using Microsoft.Data.SqlClient;
using System;
using Task6_AWS.Entities;
using Task6_AWS.Helpers;

namespace Task6_AWS
{
    public class UserRepository
    {
        public async Task AddUser(User user)
        {
            var connectionString = await DataHelper.GetConnectionString();
            LambdaLogger.Log(connectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Users(Username, Email, Company) VALUES (@Username, @Email, @Company)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Company", user.Company);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
