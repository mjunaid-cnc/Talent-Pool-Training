using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Domain.Entities;

namespace Task5.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE Email = {email}", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            var user = new User
            {
                Id = int.Parse(dt.Rows[0]["Id"].ToString()),
                Name = dt.Rows[0]["Name"].ToString(),
                Email = dt.Rows[0]["Emai"].ToString(),
                Password = dt.Rows[0]["Password"].ToString()

            };
            return user;
        }
    }
}
