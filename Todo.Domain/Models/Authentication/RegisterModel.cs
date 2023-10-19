using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Models.Authentication
{
    public class RegisterModel
    {
        [MinLength(1, ErrorMessage = "Password should at least 1 character long")]
        public string Username { get; set; } = null!;
        [MinLength(6, ErrorMessage = "Password should at least 6 characters long")]
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
