using System.Diagnostics;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
namespace HalloDoc.Controllers;

using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
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
        var myUser = _db.AspNetUsers.Where(x => x.UserName == user.UserName && x.PasswordHash == user.PasswordHash).FirstOrDefault();
        if (myUser == null)
        {
            ViewBag.message = "Login Failed";
            return View();
        }
        else
        {
            HttpContext.Session.SetString("token", user.UserName);
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}