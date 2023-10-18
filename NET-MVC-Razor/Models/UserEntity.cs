using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NET_MVC_Razor.Models
{
    public class UserEntity : IdentityUser
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; } = null!;
        [Required]
        public string Lastname { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters long")]
        public string Password { get; set; } = null!;
    }
}