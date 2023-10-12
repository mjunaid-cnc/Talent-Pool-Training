using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        [Key]
        public override Guid UserId { get; set; }
        public User User { get; set; } = null!;
        [Key]
        public override Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
