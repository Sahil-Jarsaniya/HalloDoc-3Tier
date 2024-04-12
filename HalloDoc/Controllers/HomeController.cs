using System.Diagnostics;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
namespace HalloDoc.Controllers;
using Microsoft.AspNetCore.Http;
using System.Text;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.BussinessAccess.Repository.Interface;
using NuGet.Protocol;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.IdentityModel.Tokens.Jwt;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly ILoginRepository _login;
    private readonly IJwtService _jwtService;
    private readonly IPatientRepository _patientRepo;
    private readonly INotyfService _notyf;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, ILoginRepository login, IJwtService jwtService, IPatientRepository patientRepo, INotyfService notyf)
    {
        _logger = logger;
        _db = db;
        _login = login;
        _jwtService = jwtService;
        _patientRepo = patientRepo;
        _notyf = notyf;
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
        string subject = "Reset Password";
        string body = "link";

        _login.SendEmail(asp.Email, subject, body);

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
            aspUser.PasswordHash = _login.GetHash(obj.ConfirmPassword);
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
        var hashPass = _login.GetHash(user.PasswordHash);
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
            _notyf.Success("Successful Login");
            return RedirectToAction("Dashboard", "Patient");
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

    public IActionResult ReviewAgreement(string reqClientId)
    {
        var data = _patientRepo.ReviewAgreement(reqClientId);

        return View(data);
    }
    public IActionResult Agree(int reqClientId)
    {
        _patientRepo.Agree(reqClientId);
        return RedirectToAction("login", "Home");
    }

    public IActionResult DisAgree(AgreementViewModel obj)
    {
        _patientRepo.DisAgree(obj);
        return RedirectToAction("login", "Home");
    }

    public IActionResult AccessDenied()
    {
        var token = Request.Cookies["jwt"];
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
        string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
        string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
        ViewBag.AdminName = fname + "_" + lname;    
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}