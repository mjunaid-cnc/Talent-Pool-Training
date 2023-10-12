using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Todo.Domain.Models
{
    public class ToDoModel
    {
        [Required]
        [MinLength(1, ErrorMessage = "Title should be at least 1 character long")]
        [MaxLength(30, ErrorMessage = "Title should be less than 30 characters")]
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}
