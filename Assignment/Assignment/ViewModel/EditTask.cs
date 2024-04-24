using Assignment.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment.ViewModel
{
    public class EditTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string TaskName { get; set; } = null!;

        public string? Assignee { get; set; }

        public int CategoryId { get; set; }

        public string? Description { get; set; }

        public DateOnly DueDate { get; set; }

        public string? Category { get; set; }

        public string? City { get; set; }

        public IEnumerable<Category>? categories { get; set;}
    }
}
