using System.ComponentModel.DataAnnotations;

namespace NET_MVC_Razor.Models.Domain
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public User? User { get; set; }
    }
}
