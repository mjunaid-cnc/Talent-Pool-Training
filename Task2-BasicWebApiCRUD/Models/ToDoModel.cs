using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Task2_BasicWebApiCRUD.Models
{
    public class ToDoModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Title should be at least 1 character long")]
        [MaxLength(30, ErrorMessage = "Title should be less than 30 characters")]
        public string Title { get; set; } = null!;
        [Required]
        [DefaultValue(false)]
        public bool IsCompleted { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
    }
}
