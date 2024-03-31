using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class ProvidersMenu : Controller
    {
        private readonly IAdminDashboardRepository _adminRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly ILoginRepository _loginRepo;
        private readonly ApplicationDbContext _db;
        private readonly IJwtService _jwtService;
        private readonly INotyfService _notyf;
        private readonly ISMSSender _sms;

        public ProvidersMenu(IAdminDashboardRepository adminRepo, ApplicationDbContext db, IJwtService jwtService, INotyfService notyf, ILoginRepository loginRepo, IRequestRepository requestRepo, ISMSSender sms)
        {
            _adminRepo = adminRepo;
            _db = db;
            _jwtService = jwtService;
            _notyf = notyf;
            _loginRepo = loginRepo;
            _requestRepo = requestRepo;
            _sms = sms;
        }

        public IActionResult Scheduling()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var date = DateOnly.FromDateTime(DateTime.Now);

            var data = new Scheduling
            {
                Date = date,
                Regions = _db.Regions
            };

            return View(data);
        }
        public PartialViewResult CreateShift()
        {

            var data = new CreateShift()
            {
                Regions = _db.Regions
            };
            return PartialView("_CreateShift", data);
        }

        [HttpPost]
        public void CreateShift(string selectedDays, CreateShift obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            var day = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedDays);

            var curDate = obj.Startdate;
            var curDay = (int)obj.Startdate.DayOfWeek;

            var shift = new Shift()
            {
                Physicianid = obj.Physicianid,
                Startdate = obj.Startdate,
                Isrepeat = obj.Isrepeat,
                Repeatupto = obj.Repeatupto,
                Createdby = AspId,
                Createddate = DateTime.Now,
            };
            _db.Shifts.Add(shift);
            _db.SaveChanges();


            for (int i = 1; i <= obj.Repeatupto; i++)
            {
                foreach (var item in day)
                {
                    if (item.Checked)
                    {
                        var shiftDay = 7 * i - curDay + item.Id;
                        var shiftDate = curDate.AddDays(shiftDay);

                        var shiftdetail = new Shiftdetail()
                        {
                            Shiftid = shift.Shiftid,
                            Shiftdate = shiftDate,
                            Starttime = obj.StartTime,
                            Endtime = obj.EnddTime,
                            Status = (short)_db.Physicians.FirstOrDefault(x => x.Physicianid == obj.Physicianid).Status,

                        };
                        _db.Shiftdetails.Add(shiftdetail);
                        _db.SaveChanges();

                        var shiftRegion = new Shiftdetailregion()
                        {
                            Regionid = obj.Regionid,
                            Shiftdetailid = shiftdetail.Shiftdetailid,
                        };
                        _db.Shiftdetailregions.Add(shiftRegion);
                        _db.SaveChanges();
                    }
                }
            }



        }

        public PartialViewResult DayWiseScheduling(string date)
        {
            var date1 = DateOnly.Parse(date);

            var data = from t1 in _db.Physicians
                       join t2 in _db.Shifts on t1.Physicianid equals t2.Physicianid
                            join t3 in _db.Shiftdetails on t2.Shiftid equals t3.Shiftid
                       //where t3.Shiftdate.Month == date1.Month && t3.Shiftdate.Day == date1.Day && date1.Year == t3.Shiftdate.Year
                       select new DayScheduling()
                       {
                           PhysicianId = t1.Physicianid,
                           PhysicianName = t1.Firstname + " " + t1.Lastname,
                           Shiftid = t3.Shiftid,
                           Startdate = t2.Startdate,
                           EnddTime = t3.Endtime,
                           StartTime = t3.Starttime
                       };

            return PartialView("_DayWiseScheduling", data);
        }
        public PartialViewResult WeekWiseScheduling()
        {
            return PartialView("_WeekWiseScheduling");
        }
        public PartialViewResult MonthWiseScheduling()
        {
            return PartialView("_MonthWiseScheduling");
        }
    }
}
