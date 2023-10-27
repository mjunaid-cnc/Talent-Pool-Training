using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private Database db = new Database();
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddUser(User user)
        {
            string query = "INSERT INTO Users(Name,Email,Password,Role) VALUES(@Name,@Email,@Password,@Role)";
            var parameters = new IDataParameter[]
            {
                new SqlParameter("@Name", user.Name),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@Role", user.Role)
            };
            int insertResult = db.ExecuteData(query, parameters);
            return insertResult;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            string query = $"SELECT * FROM Users WHERE Email = '{email}'";
            DataTable dt = db.GetTable(query);
            if (dt.Rows.Count > 0)
            {
                var user = new User
                {
                    Id = int.Parse(dt.Rows[0]["Id"].ToString()!),
                    Name = dt.Rows[0]["Name"].ToString()!,
                    Email = dt.Rows[0]["Email"].ToString()!,
                    Password = dt.Rows[0]["Password"].ToString()!,
                    Role = dt.Rows[0]["Role"].ToString()!
                };
                return user;
            }
            return null;
        }
    }
}
