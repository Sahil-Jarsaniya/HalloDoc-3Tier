using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using HalloDoc.Services;
using HalloDoc.DataAccess.utils;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HalloDoc.DataAccess.ViewModel.PhysicianDashboard;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class ProvidersMenu : Controller
    {
        private readonly IProviderMenuRepository _ProviderMenu;
        private readonly IPhysicianSiteRepository _phyRepo;
        private readonly INotyfService _noty;

        public ProvidersMenu(IProviderMenuRepository providerMenuRepo, INotyfService noty, IPhysicianSiteRepository phyRepo)
        {
            _ProviderMenu = providerMenuRepo;
            _noty = noty;
            _phyRepo = phyRepo;
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

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
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

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public PartialViewResult CreateShift()
        {

            var data = new CreateShift()
            {
                Regions = _ProviderMenu.Regions()
            };
            return PartialView("_CreateShift", data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public void CreateShift(string selectedDays, CreateShift obj)
        {
            string AspId = GetAdminAspId();

            _ProviderMenu.CreateShift(selectedDays, obj, AspId);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public PartialViewResult DayWiseScheduling(string date, int region)
        {

            var data = _ProviderMenu.DayWiseScheduling(date);

            if (region != 0 && region != null)
            {
                data = data.Where(x => x.regionId == region);
            }
            return PartialView("_DayWiseScheduling", data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public PartialViewResult WeekWiseScheduling(string date, int region)
        {
            var data = _ProviderMenu.WeekWiseScheduling(date);
            if (region != 0 && region != null)
            {
                data.daySchedulings = data.daySchedulings.Where(x => x.regionId == region);
            }
            return PartialView("_WeekWiseScheduling", data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public PartialViewResult MonthWiseScheduling(string date, int region)
        {
            var data = _ProviderMenu.MonthScheduling(date);
            if (region != 0 && region != null)
            {
                data.DaySchedulings = data.DaySchedulings.Where(x => x.regionId == region);
            }
            return PartialView("_MonthWiseScheduling", data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public PartialViewResult ViewShift(int shiftDetailId)
        {
            var data = _ProviderMenu.ViewShift(shiftDetailId);

            return PartialView("_ViewShift", data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public PartialViewResult ViewAllShift(string date)
        {
            var date1 = DateOnly.Parse(date);
            var data = _ProviderMenu.ViewAllShift(date);

            return PartialView("_ViewAllShift", data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public IActionResult DeleteShift(int shiftDetailId)
        {
            bool x = _ProviderMenu.DeleteShift(shiftDetailId);
            if (x)
            {
                _noty.Success("Shift Deleted.");
                return Ok(new { success = true });
            }
            else
            {
                _noty.Error("somthing went wrong");
                return Ok(new { success = true });
            }
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public IActionResult DeleteSelectedShift(int[] shiftDetailId)
        {
            var flag = false;
            for (var i = 0; i < shiftDetailId.Length; i++)
            {
                flag = _ProviderMenu.DeleteShift(shiftDetailId[i]);
            }
            if (flag)
            {
                _noty.Success("Shift Deleted.");
                return Ok(new { success = true });
            }
            else
            {
                _noty.Error("somthing went wrong");
                return Ok(new { success = true });
            }
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public IActionResult ReturnShift(int shiftDetailId)
        {
            bool x = _ProviderMenu.ReturnShift(shiftDetailId);
            if (x)
            {
                _noty.Success("Shift Approved.");
                return Ok(new { success = true });
            }
            else
            {
                _noty.Error("somthing went wrong");
                return Ok(new { success = true });
            }
        }
        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public IActionResult ReturnSelectedShift(int[] shiftDetailId)
        {
            var flag = false;
            for (var i = 0; i < shiftDetailId.Length; i++)
            {
                flag = _ProviderMenu.ReturnShift(shiftDetailId[i]);
            }
            if (flag)
            {
                _noty.Success("Shift Approved.");
                return Ok(new { success = true });
            }
            else
            {
                _noty.Error("somthing went wrong");
                return Ok(new { success = true });
            }
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
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

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public IActionResult ProviderOnCall()
        {
            ViewBag.AdminName = GetAdminName();
            var data = _ProviderMenu.ProviderOnCall();
            return View(data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public IActionResult RequestedShift()
        {
            ViewBag.AdminName = GetAdminName();
            var data = new Scheduling
            {
                Regions = _ProviderMenu.Regions()
            };
            return View(data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Scheduling)]
        public async Task<IActionResult> RequestedShiftTable(int pagenumber, int RegionFilter)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 5;
            var data = _ProviderMenu.RequestedShiftTable();
            if (RegionFilter != 0 && RegionFilter != null)
            {
                data = data.Where(x => x.RegionId == RegionFilter);
            }
            return PartialView("_RequestedShiftTable", await PaginatedList<RequestedShiftVM>.CreateAsync(data, pagenumber, pageSize));
        }

        [RoleAuth((int)enumsFile.adminRoles.ProviderLocation)]
        public IActionResult ProviderLocation()
        {
            ViewBag.AdminName = GetAdminName();

            var data = _ProviderMenu.Physicianlocation();
            return View(data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Invoicing)]
        public IActionResult Invoicing()
        {
            ViewBag.AdminName = GetAdminName();
            var data = new InvoicingVM()
            {
                physicians = _ProviderMenu.GetPhysicians()
            };
            return View(data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Invoicing)]
        public IActionResult PendingTimeSheet(string date, int phyId)
        {
            var data = _ProviderMenu.PendingTimeSheet(date, phyId);
            if (data == null)
            {
                return Ok(new { success = false });
            }
            if(data.IsApproved == true)
            {
                return Ok(new { success = true });
            }

            return PartialView("_PendingSheetTable", data);
        }
        [RoleAuth((int)enumsFile.adminRoles.Invoicing)]
        public IActionResult SheetData(string date, int phyId)
        {
            var data = _phyRepo.sheetData(date, phyId);
            return PartialView("_SheetData", data);
        }
        [RoleAuth((int)enumsFile.adminRoles.Invoicing)]
        public async Task<IActionResult> ReceiptData(string date, int pageNumber, int phyId)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 1;
            var data = _phyRepo.ReceiptData(date, phyId);
            return PartialView("_ReceiptData", await PaginatedList<BiWeeklyRecieptVM>.CreateAsync(data, pageNumber, pageSize));
        }

        [RoleAuth((int)enumsFile.adminRoles.Invoicing)]
        public IActionResult BiWeeklySheet(int id)
        {
            var data = _ProviderMenu.BiweeklySheet(id);
            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Invoicing)]
        public IActionResult BiWeeklySheet(DateVM obj)
        {
            obj.isFinal = true;
            _phyRepo.biweeklySheetVMs(obj, obj.physicianId);
            return RedirectToAction("Invoicing");
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Invoicing)]
        public IActionResult ApproveTimeSheet(int sheetId, int bonus, string note, int total)
        {
            _ProviderMenu.ApproveTimeSheet(sheetId, bonus, note, total);    
            return RedirectToAction("Invoicing");
        }

    }
}
