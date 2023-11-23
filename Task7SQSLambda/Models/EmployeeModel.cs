using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task7SQSLambda.Models
{
    public class EmployeeModel
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Salary { get; set; } = null!;
    }
}
