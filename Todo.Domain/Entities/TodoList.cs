using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Entities
{
    public class TodoList
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
        public DateTime? UpdatedDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
