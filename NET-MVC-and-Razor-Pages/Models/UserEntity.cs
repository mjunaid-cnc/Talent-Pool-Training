using System.ComponentModel.DataAnnotations;

namespace NET_MVC_and_Razor_Pages.Models
{
    public class UserEntity
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters long")]
        [Required]
        public string Password { get; set; } = null!;

    }
}
