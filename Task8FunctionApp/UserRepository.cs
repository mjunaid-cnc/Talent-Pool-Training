using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task8FunctionApp.Entities;
using Task8FunctionApp.Helpers;
using Task8FunctionApp.Models;

namespace Task8FunctionApp
{
    public class UserRepository
    {
        public async Task AddUser(UserModel user)
        {
            var connectionString = await DataHelper.GetConnectionString() ?? throw new Exception("Something went wrong");

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
