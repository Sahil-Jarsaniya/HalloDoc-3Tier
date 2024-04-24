using Assignment.Models;
using Assignment.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskManagementContext _db;
        public HomeController(ILogger<HomeController> logger, TaskManagementContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            List<Models.Task> task = _db.Tasks.ToList();

            IndexViewModel obj = new IndexViewModel()
            {
                categories = _db.Categories.ToList(),
            };
            return View(obj);
        }

        public async Task<IActionResult> FilterTable(string name, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (pageSize < 5)
            {
                pageSize = 5;
            }

            var task = (from t1 in _db.Tasks
                        select t1);
            if (name != null)
            {
                task = task.Where(x => x.TaskName.ToLower().Contains(name.ToLower()));
            }

            return PartialView("_TaskTable", await PaginatedList<Models.Task>.CreateAsync(task, pageNumber, pageSize));
        }

        public IActionResult AddTask()
        {
            EditTask data = new EditTask()
            {
                categories = _db.Categories
            };
            return PartialView("_AddTask", data);
        }

        [HttpPost]
        public IActionResult AddTask(EditTask task)
        {
            if (ModelState.IsValid)
            {

                var obj = new Models.Task()
                {
                    Description = task.Description,
                    DueDate = task.DueDate,
                    TaskName = task.TaskName,
                    City = task.City,
                    Category = _db.Categories.FirstOrDefault(x => x.Id == task.CategoryId).Name,
                    Assignee = task.Assignee,
                    CategoryId = task.CategoryId,
                };
                _db.Add(obj);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult EditTask(int id)
        {
            var task = _db.Tasks.FirstOrDefault(x => x.Id == id);
            var data = new EditTask()
            {
                Id = id,
                Assignee = task.Assignee,
                CategoryId = task.CategoryId,
                Category = task.Category,
                City = task.City,
                Description = task.Description,
                DueDate = (DateOnly)task.DueDate,
                TaskName = task.TaskName,
                categories = _db.Categories
            };

            return PartialView("_EditTask", data);
        }
        [HttpPost]
        public IActionResult EditTask(EditTask obj)
        {

            if (ModelState.IsValid)
            {
                var task = _db.Tasks.FirstOrDefault(x => x.Id == obj.Id);

                task.Assignee = obj.Assignee;
                task.CategoryId = obj.CategoryId;
                task.Category = _db.Categories.FirstOrDefault(x => x.Id == task.CategoryId).Name;
                task.Description = obj.Description;
                task.DueDate = obj.DueDate;
                task.City = obj.City;
                task.TaskName = obj.TaskName;

                _db.Tasks.Update(task);
                _db.SaveChanges();

            }

            return RedirectToAction("Index");
        }
        public IActionResult DeleteTask(int id)
        {
            var task = _db.Tasks.FirstOrDefault(x => x.Id == id);
            _db.Tasks.Remove(task);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}