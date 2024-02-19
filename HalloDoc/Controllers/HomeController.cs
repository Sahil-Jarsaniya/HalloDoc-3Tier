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

    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }
    }
    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("email", "sahil.jarsaniya@etatvasoft.com"));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;
    }

    [HttpPost]
    public IActionResult forget_password_page(AspNetUser asp)
    {
        string emailFrom = "sahil.jarsaniya@etatvasoft.com";
        string pass = "LHV0@YOA?)M";
        string SmtpServer = "mail.etatvasoft.com";
        int Port = 465;

        if (true)
        {
            //using var smtp = new MailKit.Net.Smtp.SmtpClient();
            //smtp.Connect("mail.etatvasoft.com", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate(emailFrom, pass);
            //smtp.Send(CreateEmailMessage(new Message(new string[] { forPasVM.Email }, "test", "content")));
            //smtp.Disconnect(true);
            //smtp.Dispose();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(SmtpServer, Port, true);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    //client.Authenticate(emailFrom, pass);

                    client.Send(CreateEmailMessage(new Message(new string[] { asp.Email }, "Change Your Password", "Visit https://www.google.com to change password.")));
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

            return View("Index");
        }
        else
        {
            return View();
        }

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

