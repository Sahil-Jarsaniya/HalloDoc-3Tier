using demo.Data;
using demo.ModelAccess;
//using demo.ModelAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<User> data = _db.Users;
            return View(data);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User obj)
        {
            if(ModelState.IsValid)
            {
            _db.Users.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
            }
            TempData["success"] = "Created successfully";
            return View(obj);
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var userFromDb = _db.Users.Find(id);
            //var userFromDbFirst = _db.Users.FirstOrDefault(u => u.Id == id);

            if(userFromDb == null)
            {
                return NotFound();
            }
            return View(userFromDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User obj, int id)
        {
            obj.UserId = id;

            if (ModelState.IsValid)
            {
                    _db.Users.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["success"] = "updated successfully";
            return View(obj);
        }
        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var userFromDb = _db.Users.Find(id);
            //var userFromDbFirst = _db.Users.FirstOrDefault(u => u.Id == id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Users.Find(id);  
            if(obj == null)
            {
                return NotFound();
            }
                _db.Users.Remove(obj);
                _db.SaveChanges();
            TempData["success"] = "deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
