using HalloDoc.BussinessAccess.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminDashboard : Controller
    {
        private readonly IAdminDashboardRepository _adminRepo;

        public AdminDashboard(IAdminDashboardRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public IActionResult Dashboard()
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            var data = _adminRepo.adminDashboard();
            return View(data);
        }
    }
}
