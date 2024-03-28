using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
        public PartialViewResult CreateShift()
        {
            return PartialView("_CreateShift");
        }
    }
}
