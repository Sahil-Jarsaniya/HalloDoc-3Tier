using System.Diagnostics;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
namespace HalloDoc.Controllers;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using HalloDoc.DataAccess.ViewModel;
using System.Net.Mail;
using HalloDoc.BussinessAccess.Repository.Interface;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly ILoginRepository _login;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, ILoginRepository login)
    {
        _logger = logger;
        _db = db;
        _login = login;
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

        if(obj.Password != obj.ConfirmPassword)
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
        String userName = myUser.Firstname + " " + myUser.Lastname;
            HttpContext.Session.SetString("token", userName);
            HttpContext.Session.SetInt32("userId", myUser.Userid);
            String AspId = myUser.Aspnetuserid;
            return RedirectToAction("Dashboard", "Patient", new { AspId = AspId });
        }
    }
    public IActionResult logout()
    {
        if (HttpContext.Session.GetString("token") != null)
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("login");
        }
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}