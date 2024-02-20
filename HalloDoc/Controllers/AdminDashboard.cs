using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminDashboard : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();

            return View();
        }
    }
}
