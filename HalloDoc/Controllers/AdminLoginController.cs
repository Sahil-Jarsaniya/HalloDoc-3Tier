using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace HalloDoc.Controllers
{
    public class AdminLoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILoginRepository _LoginRepository;

        public AdminLoginController(ApplicationDbContext db, ILoginRepository loginRepository)
        {
            _db = db;
            _LoginRepository = loginRepository;
        }
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(AspNetUser obj)
        {
            var hashPass = _LoginRepository.GetHash(obj.Password);
            var myUser = _LoginRepository.GetLoginData(obj, hashPass);
            if (myUser == null)
            {
                ViewBag.message = "Login Failed";
                return View();
            }
            else
            {
                
                String userName = myUser.Firstname + " " + myUser.Lastname;
                HttpContext.Session.SetString("adminToken", userName);
                HttpContext.Session.SetInt32("AdminId", myUser.Adminid);
                String AspId = myUser.Aspnetuserid;
                return RedirectToAction("Dashboard", "AdminDashboard", new { AspId = AspId });
            }
        }
        
        public IActionResult logout()
        {
            return RedirectToAction("login");
        }
    }
}
