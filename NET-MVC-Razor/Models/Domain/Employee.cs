using NET_MVC_Razor.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace NET_MVC_Razor.Models.Domain
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Salary { get; set; }
        [EmailAddress]
        [Required]
        [TestEmail]
        public string Email { get; set; }
        public User? User { get; set; }
    }
}
