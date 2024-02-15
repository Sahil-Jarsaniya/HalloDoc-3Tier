using System.Diagnostics;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
namespace HalloDoc.Controllers;

using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public static string GetHash(string text)
    {
        // SHA512 is disposable by inheritance.  
        using (var sha256 = SHA256.Create())
        {
            // Send a sample text to hash.  
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            // Get the hashed string.  
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult forget_password_page()
    {
        return View();
    }

    public IActionResult login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult login(AspNetUser user)
    {
        var hashPass = GetHash(user.Password);
        var myUser = _db.AspNetUsers.Where(x => x.UserName == user.UserName && x.Password == hashPass).FirstOrDefault();
        var userId = _db.Users.Where(x => x.Aspnetuserid == myUser.Id).FirstOrDefault();
        String userName = userId.Firstname + " " + userId.Lastname;
        if (myUser == null)
        {
            ViewBag.message = "Login Failed";
            return View();
        }
        else
        {
            HttpContext.Session.SetString("token", userName);
            String AspId = myUser.Id;          
            return RedirectToAction("Dashboard", "Patient", new { AspId = AspId});
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
    [HttpPost]
    public IActionResult ResetPassword(String email)
    {


        return RedirectToAction("login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}