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
    //[HttpPost]
    //public IActionResult forget_password_page(AspNetUser asp)
    //{
    //    var existUser = _db.AspNetUsers.Where(x => x.Email == asp.Email).FirstOrDefault();

    //    if (existUser == null)
    //    {
    //        return View();
    //    }
    //    else
    //    {
    //        TempData.Remove("email");
    //        TempData.Add("email",asp.Email);

    //        return RedirectToAction("Resetpassword");
    //    }
    //}



    [HttpPost]
    public IActionResult forget_password_page(AspNetUser asp)
    {
        var emailToSend = new MimeMessage();

        emailToSend.From.Add(MailboxAddress.Parse("tatva.dotnet.sahiljarsaniya@outlook.com"));
        emailToSend.To.Add(MailboxAddress.Parse(asp.Email));
        emailToSend.Subject = "Reset Passowrd";
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "link" };


        //send mail
        using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
        {
            emailClient.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate("tatva.dotnet.sahiljarsaniya@outlook.com","$@hilpj1");
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }

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
            aspUser.Password = GetHash(obj.ConfirmPassword);
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
        var hashPass = GetHash(user.Password);
        var myUser = _db.AspNetUsers.Where(x => x.UserName == user.UserName && x.Password == hashPass).FirstOrDefault();
        if (myUser == null)
        {
            ViewBag.message = "Login Failed";
            return View();
        }
        else
        {
        var userId = _db.Users.Where(x => x.Aspnetuserid == myUser.Id).FirstOrDefault();
        String userName = userId.Firstname + " " + userId.Lastname;
            HttpContext.Session.SetString("token", userName);
            HttpContext.Session.SetInt32("userId", userId.Userid);
            String AspId = myUser.Id;
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

