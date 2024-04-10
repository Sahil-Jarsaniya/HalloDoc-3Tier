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
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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



        public PartialViewResult DayWiseScheduling(string date, int region)
        {
            var data = _ProviderMenu.DayWiseScheduling(date);

            if (region != 0 && region != null)
            {
                data = data.Where(x => x.regionId == region);
            }
            return PartialView("_DayWiseScheduling", data);
        }
        public PartialViewResult WeekWiseScheduling(string date, int region)
        {
            var data = _ProviderMenu.WeekWiseScheduling(date);
            if (region != 0 && region != null)
            {
                data.daySchedulings = data.daySchedulings.Where(x => x.regionId == region);
            }
            return PartialView("_WeekWiseScheduling", data);
        }
        public PartialViewResult MonthWiseScheduling(string date, int region)
        {
            var data = _ProviderMenu.MonthScheduling(date);
            if (region != 0 && region != null)
            {
                data.DaySchedulings = data.DaySchedulings.Where(x=> x.regionId == region);
            }
            return PartialView("_MonthWiseScheduling", data);
        }

        public PartialViewResult ViewShift(int shiftDetailId)
        {
            var data = _ProviderMenu.ViewShift(shiftDetailId);

            return PartialView("_ViewShift", data);
        }

        public PartialViewResult ViewAllShift(string date)
        {
            var date1 = DateOnly.Parse(date);
            var data = _ProviderMenu.ViewAllShift(date);

            return PartialView("_ViewAllShift", data);
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
            bool x = _ProviderMenu.ReturnShift(shiftDetailId);
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
            ViewBag.AdminName = GetAdminName();
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            var data1 = from t1 in _db.Shiftdetails
                        join t2 in _db.Shifts on t1.Shiftid equals t2.Shiftid
                        join t3 in _db.Physicians on t2.Physicianid equals t3.Physicianid
                        where t1.Starttime <= currentTime && t1.Endtime >= currentTime && t1.Shiftdate == currentDate
                        select new ProviderOnCall()
                        {
                            Name = t3.Firstname + " " + t3.Lastname,
                            profilePhoto = t3.Photo,
                            shiftDetailId = t1.Shiftdetailid,
                            providerId = t3.Physicianid
                        };

            var data2 = from t1 in _db.Physicians
                        where t1.Status == 4
                        select new ProviderOnCall()
                        {
                            Name = t1.Firstname + " " + t1.Lastname,
                            profilePhoto = t1.Photo,
                            providerId = t1.Physicianid
                        };


            var data = new Scheduling
            {
                Regions = _ProviderMenu.Regions(),
                ProviderOnCall = data1,
                ProviderOffDuty = data2
            };
            return View(data);
        }

        public IActionResult RequestedShift()
        {
            ViewBag.AdminName = GetAdminName();
            var data = new Scheduling
            {
                Regions = _ProviderMenu.Regions()
            };
            return View(data);
        }
        public async Task<IActionResult> RequestedShiftTable(int pagenumber, int RegionFilter)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 2;
            var data = _ProviderMenu.RequestedShiftTable();
            if (RegionFilter != 0 && RegionFilter != null)
            {
                data = data.Where(x => x.RegionId == RegionFilter);
            }
            return PartialView("_RequestedShiftTable", await PaginatedList<RequestedShiftVM>.CreateAsync(data, pagenumber, pageSize));
        }

        public IActionResult ProviderLocation()
        {
            ViewBag.AdminName = GetAdminName();

            var data = _db.Physicianlocations;
            return View(data);
        }
    }
}
