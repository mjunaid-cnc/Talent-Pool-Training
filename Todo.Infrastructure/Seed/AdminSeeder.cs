using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Entities;
using Todo.Domain.Helpers;

namespace Todo.Infrastructure.Seed
{
    public class AdminSeeder
    {
        public static void SeedAdmin(AppDbContext context)
        {
            var adminUser = new User
            {
                UserName = "AdminName",
                NormalizedUserName = "ADMINNAME",
                PasswordHash = "AQAAAAEAACcQAAAAEKU0+C65T+Q5CXns0Uk/63mrYiKdPEWUUMtC9YbcbqIks91cisuERfynsDQuyn8chw=="
            };

            if (!context.Users.Any(x => x.UserName == adminUser.UserName))
            {
                context.Users.Add(adminUser);
                context.SaveChanges();

                var adminRole = context.Roles.FirstOrDefault(x => x.Name == UserRoleType.Admin.ToString());
                if (adminRole != null)
                {
                    var adminUserRole = new UserRole
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id
                    };

                    if (!context.UserRoles.Any(x => x.UserId == adminUserRole.UserId && x.RoleId == adminUserRole.RoleId))
                    {
                        context.UserRoles.Add(adminUserRole);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
