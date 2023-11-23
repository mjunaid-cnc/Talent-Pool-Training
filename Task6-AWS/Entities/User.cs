using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6_AWS.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
    }
}
