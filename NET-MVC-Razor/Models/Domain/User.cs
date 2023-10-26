using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NET_MVC_Razor.Models.Domain
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
