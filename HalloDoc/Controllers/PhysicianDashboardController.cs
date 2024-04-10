using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using Twilio.Rest.Taskrouter.V1.Workspace.TaskQueue;
using Twilio.TwiML.Voice;

namespace HalloDoc.Controllers
{
    public class PhysicianDashboardController : Controller
    {
        private readonly IAdminDashboardRepository _adminRepo;
        private readonly IPhysicianSiteRepository _phyRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly IProviderMenuRepository _providerRepo;
        private readonly ILoginRepository _loginRepo;
        private readonly ICommonRepository _common;
        private readonly ApplicationDbContext _db;
        private readonly IJwtService _jwtService;
        private readonly INotyfService _notyf;
        private readonly ISMSSender _sms;
        public PhysicianDashboardController(IPhysicianSiteRepository phyRepo, ApplicationDbContext db, IJwtService jwtService, INotyfService notyf, ILoginRepository loginRepo, IRequestRepository requestRepo, ISMSSender sms, ICommonRepository common, IProviderMenuRepository providerRepo, IAdminDashboardRepository adminRepo)
        {
            _notyf = notyf;
            _phyRepo = phyRepo;
            _db = db;
            _jwtService = jwtService;
            _loginRepo = loginRepo;
            _requestRepo = requestRepo;
            _sms = sms;
            _common = common;
            _providerRepo = providerRepo;
            _adminRepo = adminRepo;
        }
        public string GetAspID()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            return AspId;
        }
        public string GetName()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            return fname + "_" + lname;
        }
        public IActionResult Dashboard(int? status)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = GetAspID();
            ViewBag.AdminName = GetName();
            var dashData = new AdminDashboardViewModel
            {
                status = status,
                countRequestViewModel = _phyRepo.DashboardCount(_phyRepo.GetPhysicianId(AspId)),
                Region = _db.Regions
            };
            return View(dashData);
        }

        [HttpPost]
        public async Task<IActionResult> PartialTable(int status, searchViewModel? obj, int pageNumber)
        {
            var phyId = _phyRepo.GetPhysicianId(GetAspID());
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            int pageSize = 2;
            if (status == 1)
            {
                var parseData = _phyRepo.newReq(obj, phyId);
                //return PartialView("_newRequestView", parseData);
                return PartialView("_newRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == 2)
            {
                var parseData = _phyRepo.pendingReq(obj, phyId);
                return PartialView("_PendingRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == 8)
            {
                var parseData = _phyRepo.activeReq(obj, phyId);
                return PartialView("_activeRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == 4)
            {
                var parseData = _phyRepo.concludeReq(obj, phyId);
                return PartialView("_concludeReqView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }

            return View();
        }

        public IActionResult AcceptRequest(int id)
        {
            bool x = _phyRepo.AccpetRequest(id);
            if (x)
            {
                _notyf.Success("Request has been Accepted");
                return RedirectToAction("Dashboard");  
            }
            else
            {
                _notyf.Error("Something went wrong");
                return RedirectToAction("Dashboard");
            }
        }

        public IActionResult ViewCase(int reqClientId)
        {
            ViewBag.AdminName = GetName();
            var viewdata = _adminRepo.viewCase(reqClientId);

            return View(viewdata);
        }

        public IActionResult ViewNote(int reqClientId)
        {
            ViewBag.AdminName = GetName();
            var data = _adminRepo.ViewNoteGet(reqClientId);

            return View(data);
        }

        public IActionResult ViewUpload(int reqClientId)
        {
            string AspId = GetAspID();
            ViewBag.AdminName = GetName();
            int physicianId = _phyRepo.GetPhysicianId(AspId);
            var data = _adminRepo.ViewUpload(reqClientId);
            return View(data);
        }

        public IActionResult SendEmailToPatient(string FirstName, string LastName, string Email, string PhoneNumber)
        {
            var subject = "Send your request";
            var body = "<a href='/HomeController/Index+'>HalloDoc</a>";
            _loginRepo.SendEmail(Email, subject, body);
            Task<bool> val = _sms.SendSmsAsync("+91" + PhoneNumber, body);

            return RedirectToAction("Dashboard");
        }

        public IActionResult CreateRequest()
        {
            string AspId = GetAspID();
            ViewBag.AdminName = GetName();

            return View();
        }

        [HttpPost]
        public IActionResult CreateRequest(PatientViewModel obj)
        {

            if (ModelState.IsValid)
            {
                _requestRepo.CreatePatientRequest(obj);

                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public IActionResult Scheduling()
        {
            ViewBag.AdminName = GetName();

            var date = DateOnly.FromDateTime(DateTime.Now);

            var data = new Scheduling
            {
                Date = date,
                Regions = _providerRepo.Regions()
            };

            return View(data);
        }
        public PartialViewResult MonthWiseScheduling(string date)
        {
            var data = _phyRepo.monthScheduling(date, _phyRepo.GetPhysicianId(GetAspID()));

            return PartialView("_MonthWiseScheduling", data);
        }

        public PartialViewResult CreateShift()
        {
            var data = new CreateShift()
            {
                Regions = (IEnumerable<DataAccess.Models.Region>)_phyRepo.CreateShiftRegion(_phyRepo.GetPhysicianId(GetAspID()))
            };
            return PartialView("_CreateShift", data);
        }

        [HttpPost]
        public void CreateShift(string selectedDays, CreateShift obj)
        {
            string AspId = GetAspID();
            obj.Physicianid = _phyRepo.GetPhysicianId(AspId);
            _providerRepo.CreateShift(selectedDays, obj, AspId);
        }

        public PartialViewResult ViewShift(int shiftDetailId)
        {
            var data = _providerRepo.ViewShift(shiftDetailId);
            data.Regions = _phyRepo.CreateShiftRegion(_phyRepo.GetPhysicianId(GetAspID()));
            return PartialView("_ViewShift", data);
        }

        public PartialViewResult ViewAllShift(string date)
        {
            var data = _phyRepo.ViewAllShift(date, _phyRepo.GetPhysicianId(GetAspID()));
            return PartialView("_ViewAllShift", data);
        }
        public IActionResult DeleteShift(int shiftDetailId)
        {
            bool x = _providerRepo.DeleteShift(shiftDetailId);
            if (x)
            {
                _notyf.Success("Shift Deleted");
                return Ok(new { success = true });
            }
            else
            {
                _notyf.Error("somthing went wrong");
                return Ok(new { success = true });
            }
        }
        public IActionResult ReturnShift(int shiftDetailId)
        {
            bool x = _providerRepo.ReturnShift(shiftDetailId);
            if (x)
            {
                _notyf.Success("Shift Deleted");
                return Ok(new { success = true });
            }
            else
            {
                _notyf.Error("somthing went wrong");
                return Ok(new { success = true });
            }
        }
        public IActionResult UpdateShift(CreateShift obj, int id)
        {
            obj.Physicianid = _phyRepo.GetPhysicianId(GetAspID());
            bool x = _providerRepo.UpdateShift(obj, id);

            if (x)
            {
                _notyf.Success("Updated");

            }
            else
            {
                _notyf.Error("something went wrong");
            }
            return RedirectToAction("Scheduling");
        }

        public IActionResult MyProfile()
        {
            ViewBag.AdminName = GetName();
            var phyId = _phyRepo.GetPhysicianId(GetAspID());
            var data = _adminRepo.EditProvider(phyId);

            return View(data);
        }
    }
}
