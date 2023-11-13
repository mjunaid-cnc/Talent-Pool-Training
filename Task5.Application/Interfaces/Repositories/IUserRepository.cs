using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Domain.Entities;

namespace Task5.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
