using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminLoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILoginRepository _LoginRepository;
        private readonly IJwtService _JwtService;
        private readonly INotyfService _notyf;
        public AdminLoginController(ApplicationDbContext db, ILoginRepository loginRepository, IJwtService jwtService,INotyfService notyf )
        {
            _notyf = notyf;
            _db = db;
            _LoginRepository = loginRepository;
            _JwtService = jwtService;
        }
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(AspNetUser obj)
        {
            var hashPass = _LoginRepository.GetHash(obj.PasswordHash);
            var myUser = _LoginRepository.GetLoginData(obj, hashPass);
            if (myUser == null)
            {
                //ViewBag.message = "Login Failed";
                _notyf.Error("Login Failed");
                return View();
            }
            else
            {
                var user2 = new LoggedUser
                {
                    AspId = myUser.Aspnetuserid,
                    FirstName = myUser.Firstname,
                    LastName = myUser.Lastname,
                    Email = myUser.Email,
                    Role = "Admin"
                };

                var jwtToken = _JwtService.GenerateJwtToken(user2);
                Response.Cookies.Append("jwt", jwtToken);

                _notyf.Success("Successful Login");
                return RedirectToAction("Dashboard", "AdminDashboard");
            }
        }
        
        public IActionResult logout()
        {

            if (Request.Cookies["jwt"] != null)
            {
                Response.Cookies.Delete("jwt");
            };
            _notyf.Success("Successful Logout");
            
            return RedirectToAction("login");
        }
    }
}
