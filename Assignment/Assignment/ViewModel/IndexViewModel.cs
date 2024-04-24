using Assignment.Models;
namespace Assignment.ViewModel
{
    public class IndexViewModel
    {
        public IQueryable<Models.Task>? Tasks { get; set; }

        public Models.Task? AddTask { get; set; }

        public List<Category>? categories { get; set; }
    }
}
