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

            if(task)
            {
                ViewBag.success = "updated successfully";
            return RedirectToAction( "ViewCase" ,new { reqClientId = obj.Requestclientid });
            }
            else
            {
                ViewBag.error = "Error Occured!!!!";
                return RedirectToAction("ViewCase", new { reqClientId = obj.Requestclientid });
            }

        }
    }
}
