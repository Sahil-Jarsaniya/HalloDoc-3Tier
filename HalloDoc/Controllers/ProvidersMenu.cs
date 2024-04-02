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

        public ProvidersMenu(IProviderMenuRepository providerMenuRepo, ApplicationDbContext db , INotyfService noty)
        {
            _ProviderMenu = providerMenuRepo;
            _db = db;
            _noty = noty;
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
            if (obj.Isrepeat == false)
            {
                var shiftdetail = new Shiftdetail()
                {
                    Shiftid = shift.Shiftid,
                    Shiftdate = obj.Startdate,
                    Starttime = obj.StartTime,
                    Endtime = obj.EndTime,

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
            else
            {
                for (int i = 1; i <= obj.Repeatupto; i++)
                {
                    foreach (var item in day)
                    {
                        if (item.Checked)
                        {
                            var shiftDay = 7 * i - curDay + item.Id;
                            if (shiftDay == 7)
                            {
                                shiftDay = 0;
                            }
                            var shiftDate = curDate.AddDays(shiftDay);
                            var shiftdetail = new Shiftdetail()
                            {
                                Shiftid = shift.Shiftid,
                                Shiftdate = shiftDate,
                                Starttime = obj.StartTime,
                                Endtime = obj.EndTime,

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
        }



        public PartialViewResult DayWiseScheduling(string date)
        {
            var date1 = DateOnly.Parse(date);

            var data = (from t1 in _db.Physicians
                        join t2 in _db.Shifts
                        on t1.Physicianid equals t2.Physicianid into physicianShifts
                        from t2 in physicianShifts.DefaultIfEmpty()
                        join t3 in _db.Shiftdetails
                        .Where(x => x.Shiftdate.Month == date1.Month && x.Shiftdate.Day == date1.Day && x.Shiftdate.Year == date1.Year && x.Isdeleted != true)
                        on t2.Shiftid equals t3.Shiftid into shiftDetails
                        from t3 in shiftDetails.DefaultIfEmpty()
                        select new DayScheduling()
                        {
                            PhysicianId = t1.Physicianid,
                            PhysicianName = t1.Firstname + " " + t1.Lastname,
                            Shiftid = t2 != null ? t2.Shiftid : null,
                            shiftDetailId = t3 != null ? t3.Shiftdetailid : null,
                            Startdate = t2 != null ? t2.Startdate : null,
                            EndTime = t3 != null ? t3.Endtime : null,
                            StartTime = t3 != null ? t3.Starttime : null,
                            SelectedDate = date1,
                            ShiftDate = t3 != null ? t3.Shiftdate : null,
                            status = t3 != null ? t3.Status : null,
                        }).OrderBy(d => d.PhysicianId);
            return PartialView("_DayWiseScheduling", data);
        }
        public PartialViewResult WeekWiseScheduling(string date)
        {
            var date1 = DateOnly.Parse(date);
            var day = from t1 in _db.Physicians
                       join t2 in _db.Shifts
                       on t1.Physicianid equals t2.Physicianid into physicianShifts
                       from t2 in physicianShifts.DefaultIfEmpty()
                       join t3 in _db.Shiftdetails
                       .Where(x => x.Shiftdate.Month == date1.Month && x.Shiftdate.Day == date1.Day && x.Shiftdate.Year == date1.Year && x.Isdeleted != true)
                       on t2.Shiftid equals t3.Shiftid into shiftDetails
                       from t3 in shiftDetails.DefaultIfEmpty()
                       select new DayScheduling()
                       {
                           PhysicianId = t1.Physicianid,
                           PhysicianName = t1.Firstname + " " + t1.Lastname,
                           Shiftid = t2 != null ? t2.Shiftid : null,
                           shiftDetailId = t3 != null ? t3.Shiftdetailid : null,
                           Startdate = t2 != null ? t2.Startdate : null,
                           EndTime = t3 != null ? t3.Endtime : null,
                           StartTime = t3 != null ? t3.Starttime : null,
                           SelectedDate = date1,
                           ShiftDate = t3 != null ? t3.Shiftdate : null,
                           status = t3 != null ? t3.Status : null,
                       };
            var data = new WeekScheduling()
            {
                physicians = _db.Physicians,
                shifts = _db.Shifts,
                shiftdetails = _db.Shiftdetails.Where(x => x.Shiftdate > date1 && x.Shiftdate < date1.AddDays(7)),
                Selecteddate = date1,
                daySchedulings = day
            };
            return PartialView("_WeekWiseScheduling", data);
        }
        public PartialViewResult MonthWiseScheduling()
        {
            return PartialView("_MonthWiseScheduling");
        }

        public PartialViewResult ViewShift(int shiftDetailId)
        {
            var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftDetailId);
            var shift = _db.Shifts.FirstOrDefault(x => x.Shiftid == shiftDetail.Shiftid);
            var shiftReg = _db.Shiftdetailregions.FirstOrDefault(x => x.Shiftdetailid == shiftDetail.Shiftdetailid);
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == shift.Physicianid);

            var data = new CreateShift()
            {
                Physicianid = phy.Physicianid,
                PhysicianName = phy.Firstname + " " + phy.Lastname,
                Regionid = shiftReg.Regionid,
                Startdate = shiftDetail.Shiftdate,
                StartTime = shiftDetail.Starttime,
                EndTime = shiftDetail.Endtime,
                Shiftid = shiftDetail.Shiftid,
                Regions = _db.Regions,
                ShiftDetailId = shiftDetail.Shiftdetailid,
            };

            return PartialView("_ViewShift", data);
        }

        public IActionResult DeleteShift(int shiftDetailId)
        {
            var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftDetailId);
            shiftDetail.Isdeleted = true;
            _db.Shiftdetails.Update(shiftDetail);
            _db.SaveChanges();
            return Ok(new { success = true });
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
            var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == id);
            shiftDetail.Shiftdate = obj.Startdate;
            shiftDetail.Endtime = obj.EndTime;
            shiftDetail.Starttime = obj.StartTime;
            shiftDetail.Modifieddate = DateTime.Now;
            shiftDetail.Regionid = obj.Regionid;
            _db.Shiftdetails.Update(shiftDetail);
    
            var shift  = _db.Shifts.FirstOrDefault(x => x.Shiftid == shiftDetail.Shiftid);
            shift.Physicianid =obj.Physicianid;
            _db.Shifts.Update(shift);

            _db.SaveChanges();

            _noty.Success("Updated");
            return RedirectToAction("Scheduling");
        }

        public IActionResult ProviderOnCall()
        {
            return View();
        }
    }
}
