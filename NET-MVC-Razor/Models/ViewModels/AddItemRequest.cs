using System.ComponentModel.DataAnnotations;

namespace NET_MVC_Razor.Models.ViewModels
{
    public class AddItemRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "Title should be less than 30 and greater than 1 character", MinimumLength = 1)]
        public string Title { get; set; } = null!;
    }
}
