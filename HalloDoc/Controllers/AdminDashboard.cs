using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HalloDoc.Controllers
{
    public class AdminDashboard : Controller
    {
        private readonly IAdminDashboardRepository _adminRepo;
        private readonly ApplicationDbContext _db;

        public AdminDashboard(IAdminDashboardRepository adminRepo, ApplicationDbContext db)
        {
            _adminRepo = adminRepo;
            _db = db;
        }

        public IActionResult Dashboard()
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            var data = _adminRepo.adminDashboard();

            return View(data);
        }
        [HttpPost]
        public IActionResult Dashboard(searchViewModel obj)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            var data = _adminRepo.adminDashboard();

            var searchedData = _adminRepo.searchPatient(obj, data);

            return View(searchedData);

        }

        public IActionResult ViewCase(int reqClientId)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");

            var viewdata = _adminRepo.viewCase(reqClientId);

            return View(viewdata);
        }

        [HttpPost]
        public IActionResult ViewCase(viewCaseViewModel obj)
        {

            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");


            bool task = _adminRepo.viewCase(obj);

            if (task)
            {
                ViewBag.success = "updated successfully";
                return RedirectToAction("ViewCase", new { reqClientId = obj.Requestclientid });
            }
            else
            {
                ViewBag.error = "Error Occured!!!!";
                return RedirectToAction("ViewCase", new { reqClientId = obj.Requestclientid });
            }

        }

        public IActionResult ViewNote(int reqClientId)
        {
            var ReqId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqNotes = from t1 in _db.Requestnotes
                           where t1.Requestid == ReqId.Requestid
                           select t1 ;

            var transferNote = from t1 in _db.Requeststatuslogs
                               where t1.Requestid == ReqId.Requestid
                               select t1;

            var data = new viewNoteViewModel
            {
                Requestnote = reqNotes,
                Requeststatuslog = transferNote
            };

            return View(data);
        }
    }
}
