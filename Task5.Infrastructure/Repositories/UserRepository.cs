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
        public async Task<dynamic> GetUserByEmail(string email)
        {
            string query = $"SELECT * FROM Users WHERE Email = '{email}'";
            DataTable dt = db.GetTable(query);
            var userCount = dt.Rows.Count;
            return userCount;
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
    }
}
