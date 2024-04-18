    using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using HalloDoc.Services;
using HalloDoc.DataAccess.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using NuGet.Protocol;
using Rotativa.AspNetCore;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using Twilio.Rest.Taskrouter.V1.Workspace.TaskQueue;
using Twilio.TwiML.Voice;

namespace HalloDoc.Controllers
{
    [CustomAuth("Provider")]
    public class PhysicianDashboardController : Controller
    {
        private readonly IAdminDashboardRepository _adminRepo;
        private readonly IPhysicianSiteRepository _phyRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly IProviderMenuRepository _providerRepo;
        private readonly ILoginRepository _loginRepo;
        private readonly ICommonRepository _common;
        private readonly IJwtService _jwtService;
        private readonly INotyfService _notyf;
        private readonly ISMSSender _sms;
        public PhysicianDashboardController(IPhysicianSiteRepository phyRepo, IJwtService jwtService, INotyfService notyf, ILoginRepository loginRepo, IRequestRepository requestRepo, ISMSSender sms, ICommonRepository common, IProviderMenuRepository providerRepo, IAdminDashboardRepository adminRepo)
        {
            _notyf = notyf;
            _phyRepo = phyRepo;
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

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public void PhysicianLocationUpdate(double latitude, double longitude)
        {
            bool x = _phyRepo.PhysicianLocationUpdate(latitude, longitude, _phyRepo.GetPhysicianId(GetAspID()));    
        }

        #region PhysicianDashboard

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
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
                Region = _phyRepo.GetRegion()
            };
            return View(dashData);
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        [HttpPost]
        public async Task<IActionResult> PartialTable(int status, searchViewModel? obj, int pageNumber)
        {
            var phyId = _phyRepo.GetPhysicianId(GetAspID());
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            int pageSize = 2;
            if (status == (int)enumsFile.DashboardStatus.newStatus)
            {
                var parseData = _phyRepo.newReq(obj, phyId);
                //return PartialView("_newRequestView", parseData);
                return PartialView("_newRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == (int)enumsFile.DashboardStatus.pending)
            {
                var parseData = _phyRepo.pendingReq(obj, phyId);
                return PartialView("_PendingRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == (int)enumsFile.DashboardStatus.active)
            {
                var parseData = _phyRepo.activeReq(obj, phyId);
                return PartialView("_activeRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == (int)enumsFile.DashboardStatus.conclude)
            {
                var parseData = _phyRepo.concludeReq(obj, phyId);
                return PartialView("_concludeReqView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }

            return View();
        }


        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult SendEmailToPatient(string FirstName, string LastName, string Email, string PhoneNumber)
        {
            var subject = "Send your request";
            var body = "<a href='/HomeController/Index+'>HalloDoc</a>";
            _loginRepo.SendEmail(Email, subject, body);
            Task<bool> val = _sms.SendSmsAsync("+91" + PhoneNumber, body);

            return RedirectToAction("Dashboard");
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult CreateRequest()
        {
            string AspId = GetAspID();
            ViewBag.AdminName = GetName();
            var data = new PatientViewModel()
            {
                Regions = _requestRepo.Regions()
            };
            return View(data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult CreateRequest(PatientViewModel obj)
        {

            if (ModelState.IsValid)
            {
                _requestRepo.CreateRequestByPhysician(obj, _phyRepo.GetPhysicianId(GetAspID()));

                return RedirectToAction("Dashboard");
            }
            return View();
        }

        #endregion

        #region DashboardAction

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
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

        [HttpPost]
        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult TransferCase(string addNote, int reqClientId)
        {
            _phyRepo.TransferCase(reqClientId, addNote);
            return Ok(new {success = true});    
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult ViewCase(int reqClientId)
        {
            ViewBag.AdminName = GetName();
            var viewdata = _adminRepo.viewCase(reqClientId);

            return View(viewdata);
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult ViewNote(int reqClientId)
        {
            ViewBag.AdminName = GetName();
            var data = _adminRepo.ViewNoteGet(reqClientId);

            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult ViewNote(string adminNote, int reqClientId)
        {
            string aspId = GetAspID();
            int phyId = _phyRepo.GetPhysicianId(GetAspID());
            _phyRepo.ViewNotePost(reqClientId, adminNote, phyId, aspId);

            return RedirectToAction("ViewNote", new { reqClientId = reqClientId });
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult ViewUpload(int reqClientId)
        {
            string AspId = GetAspID();
            ViewBag.AdminName = GetName();
            int physicianId = _phyRepo.GetPhysicianId(AspId);
            var data = _adminRepo.ViewUpload(reqClientId);
            return View(data);
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult UploadDocument(UploadFileViewModel obj)
        {
            var phyId = _phyRepo.GetPhysicianId(GetAspID());
            if (obj.formFile != null && obj.formFile.Length > 0)
            {
                _loginRepo.uploadFile(obj.formFile, "RequestData\\" + obj.reqId, obj.formFile.FileName.ToString());
                _phyRepo.viewUplodPost(obj.formFile.Name, obj.reqId, phyId);
                _notyf.Success("File uploaded.");
            }
            else
            {
                _notyf.Error("Select File First.");
            }
            return RedirectToAction("ViewUpload", new { reqClientId = obj.reqId });
        }

        [RoleAuth((int)enumsFile.physicianRoles.SendOrders)]
        public IActionResult SendOrders(int reqClientId)
        {
            ViewBag.AdminName = GetName();

            var data = _adminRepo.SendOrders(reqClientId);

            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.physicianRoles.SendOrders)]
        public IActionResult SendOrders(SendOrderViewModel obj)
        {
            string AspId = GetAspID();

            _adminRepo.SendOrders(obj, AspId);

            return RedirectToAction("SendOrders", new { reqClientId = obj.reqClientId });
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult EncounterPopUp()
        {
            return PartialView("_EncounterPopUp");
        }
        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult Encounter(int reqClientId, string option)
        {

            bool x = _phyRepo.Encounter(reqClientId, option);
            if (x)
            {
                _notyf.Success("Updated");
                return RedirectToAction("Dashboard");
            }
            else
            {
                _notyf.Success("Error");

                return RedirectToAction("Dashboard");
            }

        }
        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult HouseCallBtn(int id)
        {
            bool x = _phyRepo.HouseCallBtn(id);
            if (x)
            {
                _notyf.Success("Updated");
                return RedirectToAction("Dashboard");
            }
            else
            {
                _notyf.Success("Error");

                return RedirectToAction("Dashboard");
            }
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult EncounterForm(int reqClientId)
        {
            ViewBag.AdminName = GetName();
            var obj = _adminRepo.Encounter(reqClientId);
            if (obj.Isfinalized != true)
            {
                return View(obj);
            }
            return View();
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult isFinalizedform(int id)
        {
            var obj = _adminRepo.Encounter(id);
            if (obj.Isfinalized != true)
            {
                return Ok(new { success = false });
            }
            else
            {
                return PartialView("_FinalizedPopUp", obj);
            }
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult EncounterForm(Encounter obj)
        {
            _adminRepo.Encounter(obj);
            return View(obj);
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult FinalizeEncounter(int id)
        {
            var x = _phyRepo.FinalizeEncounter(id);
            if (x)
            {
                _notyf.Success("Updated");
                return RedirectToAction("Dashboard");
            }
            else
            {
                _notyf.Success("Error");

                return RedirectToAction("Dashboard");
            }
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult downloadFinalEncounter(int id)
        {
            Encounter obj = _phyRepo.Encounter(id);

            return new ViewAsPdf("PDF", obj)
            {
                FileName = "FinalizedEncounterForm.pdf"
            };  
        }

        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult ConcludeCare(int reqClientId)
        {
            string AspId = GetAspID();
            ViewBag.AdminName = GetName();
            var data = _adminRepo.CloseCase(reqClientId);
            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.physicianRoles.Dashboard)]
        public IActionResult ConcludeCare(CloseCaseViewModel obj)
        {
            _phyRepo.ConcludeCare(obj, GetAspID());
            _loginRepo.uploadFile(obj.fileName, "RequestData\\" + obj.ReqClientId, obj.fileName.FileName.ToString());
            return RedirectToAction("Dashboard");
        }

        #endregion

        #region Scheduling

        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
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
        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
        public PartialViewResult MonthWiseScheduling(string date)
        {
            var data = _phyRepo.monthScheduling(date, _phyRepo.GetPhysicianId(GetAspID()));

            return PartialView("_MonthWiseScheduling", data);
        }

        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
        public PartialViewResult CreateShift()
        {
            var data = new CreateShift()
            {
                Regions = (IEnumerable<DataAccess.Models.Region>)_phyRepo.CreateShiftRegion(_phyRepo.GetPhysicianId(GetAspID()))
            };
            return PartialView("_CreateShift", data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
        public void CreateShift(string selectedDays, CreateShift obj)
        {
            string AspId = GetAspID();
            obj.Physicianid = _phyRepo.GetPhysicianId(AspId);
            _providerRepo.CreateShift(selectedDays, obj, AspId);
        }

        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
        public PartialViewResult ViewShift(int shiftDetailId)
        {
            var data = _providerRepo.ViewShift(shiftDetailId);
            data.Regions = _phyRepo.CreateShiftRegion(_phyRepo.GetPhysicianId(GetAspID()));
            return PartialView("_ViewShift", data);
        }

        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
        public PartialViewResult ViewAllShift(string date)
        {
            var data = _phyRepo.ViewAllShift(date, _phyRepo.GetPhysicianId(GetAspID()));
            return PartialView("_ViewAllShift", data);
        }

        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
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

        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
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

        [RoleAuth((int)enumsFile.physicianRoles.MySchedule)]
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
        [RoleAuth((int)enumsFile.physicianRoles.MyProfile)]

        #endregion

        #region MyProfile
        public IActionResult MyProfile()
        {
            ViewBag.AdminName = GetName();
            var phyId = _phyRepo.GetPhysicianId(GetAspID());
            var data = _adminRepo.EditProvider(phyId);

            return View(data);
        }
        [RoleAuth((int)enumsFile.physicianRoles.MyProfile)]
        public IActionResult SendRequestToEdit(string Message)
        {
            var phyId = _phyRepo.GetPhysicianId(GetAspID());
            var admins = _phyRepo.GetAdminList();

            foreach(var admin in admins)
            {
                _loginRepo.SendEmail(admin.Email, "Edit My Profile", Message);
            }

            return Ok(new { success = true });
        }
        #endregion

    }
}
