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
using HalloDoc.BussinessAccess.Repository.Implementation;
using Org.BouncyCastle.Ocsp;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILoginRepository _login;
    private readonly IJwtService _jwtService;
    private readonly IPatientRepository _patientRepo;
    private readonly INotyfService _notyf;
    public HomeController(ILogger<HomeController> logger,  ILoginRepository login, IJwtService jwtService, IPatientRepository patientRepo, INotyfService notyf)
    {
        _logger = logger;
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
        var AspId = _login.GetAspId(asp.Email);
        string subject = "Reset Password";
        string body = "<a href='/Home/ResetPassword?AspId=" + AspId + "'>Reset Password link</a>";

        _login.SendEmail(asp.Email, subject, body);

        return RedirectToAction("login");
    }

    public IActionResult ResetPassword(string AspId)
    {
        var asp = _login.asp(AspId);
        var obj = new ResetPassword()
        {
            Id = AspId,
            email = asp.Email
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult ResetPassword(ResetPassword obj)
    {

        if (obj.Password != obj.ConfirmPassword || obj.Password == null || obj.ConfirmPassword == null)
        {
            _notyf.Error("Enter Correct Password");
            return View(obj);
        }
        else
        {
            try
            {
                _login.ResetPassword(obj);
                _notyf.Success("Password Changed.");
            }
            catch
            {
                _notyf.Error("Something went Wrong!!");
            }
            return RedirectToAction("login", "Home");
        }
    }

    public IActionResult login()
    {
        if (Request.Cookies["jwt"] != null)
        {
            Response.Cookies.Delete("jwt");
        };

        if (Request.Cookies["menuList"] != null)
        {
            Response.Cookies.Delete("menuList");
        };
        return View();
    }
    [HttpPost]
    public IActionResult login(login obj)
    {
        var hashPass = _login.GetHash(obj.Password);
        var myUser = _login.GetLoginData(obj, hashPass);
        if (myUser == null)
        {
            _notyf.Error("Login Failed");
            return View();
        }
        else
        {
            var isAdmin = _login.isAdmin(myUser.Id);
            if (isAdmin != null)
            {

                var user2 = new LoggedUser
                {
                    AspId = isAdmin.Aspnetuserid,
                    FirstName = isAdmin.Firstname,
                    LastName = isAdmin.Lastname,
                    Email = isAdmin.Email,
                    Role = "Admin",
                    Roleid = isAdmin.Roleid.ToString(),
                };
                var jwtToken = _jwtService.GenerateJwtToken(user2);
                Response.Cookies.Append("jwt", jwtToken);


                var menus = _login.rolemenus((int)isAdmin.Roleid);
                var menuList = "";
                foreach (var menu in menus)
                {
                    menuList = menuList + menu.Menuid + ",";
                }

                Response.Cookies.Append("menuList", menuList);
                _notyf.Success("Successful Login");
                return RedirectToAction("Dashboard", "AdminDashboard");
            }
            var isPhysician = _login.isPhysician(myUser.Id);
            if (isPhysician != null)
            {
                var user3 = new LoggedUser
                {
                    AspId = isPhysician.Aspnetuserid,
                    FirstName = isPhysician.Firstname,
                    LastName = isPhysician.Lastname,
                    Email = isPhysician.Email,
                    Role = "Provider",
                    Roleid = isPhysician.Roleid.ToString()
                };
                var jwtToken = _jwtService.GenerateJwtToken(user3);
                Response.Cookies.Append("jwt", jwtToken);

                var menus = _login.rolemenus((int)isPhysician.Roleid);
                var menuList = "";
                foreach (var menu in menus)
                {
                    menuList = menuList + "," + menu.Menuid;
                }

                Response.Cookies.Append("menuList", menuList);

                _notyf.Success("Successful Login");
                return RedirectToAction("Dashboard", "PhysicianDashboard");
            }

            var isPatient = _login.isPatient(myUser.Id);
            if (isPatient != null)
            {
                var user3 = new LoggedUser
                {
                    AspId = isPatient.Aspnetuserid,
                    FirstName = isPatient.Firstname,
                    LastName = isPatient.Lastname,
                    Email = isPatient.Email,
                    Role = "patient",
                    Roleid = "0"
                };
                var jwtToken = _jwtService.GenerateJwtToken(user3);
                Response.Cookies.Append("jwt", jwtToken);


                _notyf.Success("Successful Login");
                return RedirectToAction("Dashboard", "Patient");
            }

            _notyf.Error("Login Failed");
            return View();
        }
    }

    public IActionResult logout()
    {

        if (Request.Cookies["jwt"] != null)
        {
            Response.Cookies.Delete("jwt");
        };

        if (Request.Cookies["menuList"] != null)
        {
            Response.Cookies.Delete("menuList");
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