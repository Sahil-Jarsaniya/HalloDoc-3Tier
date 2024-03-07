using System.Diagnostics;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
namespace HalloDoc.Controllers;
using Microsoft.AspNetCore.Http;
using System.Text;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.BussinessAccess.Repository.Interface;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly ILoginRepository _login;
    private readonly IJwtService _jwtService;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, ILoginRepository login, IJwtService jwtService)
    {
        _logger = logger;
        _db = db;
        _login = login;
        _jwtService = jwtService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult forget_password_page()
    {
        return View();
    }


    [HttpPost]
    public IActionResult forget_password_page(AspNetUser asp)
    {
        _login.SendEmail(asp.Email);

        return RedirectToAction("login");
    }

    public IActionResult ResetPassword()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ResetPassword(ResetPassword obj)
    {
        string email = TempData["email"].ToString();
        var aspUser = _db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();

        if (obj.Password != obj.ConfirmPassword)
        {
            return View();
        }
        else
        {
            aspUser.Password = _login.GetHash(obj.ConfirmPassword);
            //aspUser.Password = obj.ConfirmPassword;
            _db.AspNetUsers.Update(aspUser);
            _db.SaveChanges();


            return RedirectToAction("login", "Home");
        }
    }

    public IActionResult login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult login(AspNetUser user)
    {
        var hashPass = _login.GetHash(user.Password);
        var myUser = _login.PatientLogin(user, hashPass);

        if (myUser == null)
        {
            ViewBag.message = "Login Failed";
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
                Role = "patient"
            };

            var jwtToken = _jwtService.GenerateJwtToken(user2);
            Response.Cookies.Append("jwt", jwtToken);

            String userName = myUser.Firstname + " " + myUser.Lastname;

            HttpContext.Session.SetString("token", userName);
            HttpContext.Session.SetInt32("userId", myUser.Userid);
            String AspId = myUser.Aspnetuserid;
            return RedirectToAction("Dashboard", "Patient", new { AspId = AspId });
        }
    }
    public IActionResult logout()
    {
        if (Request.Cookies["jwt"] != null)
        {
            Response.Cookies.Delete("jwt");

            return RedirectToAction("login");

        };
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}