using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using HalloDoc.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class ProvidersMenu : Controller
    {
        private readonly IProviderMenuRepository _ProviderMenu;
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _noty;

        public ProvidersMenu(IProviderMenuRepository providerMenuRepo, ApplicationDbContext db, INotyfService noty)
        {
            _ProviderMenu = providerMenuRepo;
            _db = db;
            _noty = noty;
        }
        public string GetAdminName()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            return fname + "_" + lname;
        }

        public string GetAdminAspId()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            return AspId;
        }
        public IActionResult Scheduling()
        {

            ViewBag.AdminName = GetAdminName();

            var date = DateOnly.FromDateTime(DateTime.Now);

            var data = new Scheduling
            {
                Date = date,
                Regions = _ProviderMenu.Regions()
            };

            return View(data);
        }
        public PartialViewResult CreateShift()
        {

            var data = new CreateShift()
            {
                Regions = _ProviderMenu.Regions()
            };
            return PartialView("_CreateShift", data);
        }

        [HttpPost]
        public void CreateShift(string selectedDays, CreateShift obj)
        {
            string AspId = GetAdminAspId();

            _ProviderMenu.CreateShift(selectedDays, obj, AspId);
        }



        public PartialViewResult DayWiseScheduling(string date)
        {
            var data = _ProviderMenu.DayWiseScheduling(date);
            return PartialView("_DayWiseScheduling", data);
        }
        public PartialViewResult WeekWiseScheduling(string date)
        {
            var data = _ProviderMenu.WeekWiseScheduling(date);
            return PartialView("_WeekWiseScheduling", data);
        }
        public PartialViewResult MonthWiseScheduling()
        {
            return PartialView("_MonthWiseScheduling");
        }

        public PartialViewResult ViewShift(int shiftDetailId)
        {
            var data = _ProviderMenu.ViewShift(shiftDetailId);

            return PartialView("_ViewShift", data);
        }

        public IActionResult DeleteShift(int shiftDetailId)
        {
            bool x = _ProviderMenu.DeleteShift(shiftDetailId);
            if (x)
            {
                _noty.Success("Shift Deleted");
                return Ok(new { success = true });
            }
            else
            {
                _noty.Error("somthing went wrong");
                return Ok(new { success = true });
            }
        }
        public IActionResult ReturnShift(int shiftDetailId)
        {
            var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftDetailId);
            shiftDetail.Status = 2;
            _db.Shiftdetails.Update(shiftDetail);
            _db.SaveChanges();
            return Ok(new { success = true });
        }
        public IActionResult UpdateShift(CreateShift obj, int id)
        {
            bool x = _ProviderMenu.UpdateShift(obj, id);

            if (x)
            {
                _noty.Success("Updated");

            }
            else
            {
                _noty.Error("something went wrong");
            }
            return RedirectToAction("Scheduling");
        }

        public IActionResult ProviderOnCall()
        {
            return View();
        }
    }
}
