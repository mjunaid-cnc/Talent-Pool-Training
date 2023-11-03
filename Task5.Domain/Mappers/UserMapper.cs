using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Domain.Entities;
using Task5.Domain.Models;

namespace Task5.Domain.Mappers
{
    public class UserMapper
    {
        public static User MapNewUser(RegisterRequestModel registerRequest, string password)
        {
            var user = new User
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                Password = password,
                Role = registerRequest.Role
            };
            return user;
        }
    }
}
