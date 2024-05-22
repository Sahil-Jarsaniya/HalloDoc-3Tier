using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.Services;
using HalloDoc.DataAccess.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.JSInterop.Implementation;
using System.Security.Claims;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class AdminDashboard : Controller
    {
        private readonly IAdminDashboardRepository _adminRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly ILoginRepository _loginRepo;
        private readonly ICommonRepository _common;
        private readonly IJwtService _jwtService;
        private readonly INotyfService _notyf;
        private readonly ISMSSender _sms;

        public AdminDashboard(IAdminDashboardRepository adminRepo, IJwtService jwtService, INotyfService notyf, ILoginRepository loginRepo, IRequestRepository requestRepo, ISMSSender sms, ICommonRepository common)
        {
            _notyf = notyf;
            _adminRepo = adminRepo;
            _jwtService = jwtService;
            _loginRepo = loginRepo;
            _requestRepo = requestRepo;
            _sms = sms;
            _common = common;
        }


        #region Dashboard


        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult Dashboard(int? status)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var data = _adminRepo.adminDashboard();
            var dashData = new AdminDashboardViewModel
            {
                status = status,
                countRequestViewModel = data.countRequestViewModel,
                Casetag = data.Casetag,
                Region = data.Region,
            };
            return View(dashData);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult Dashboard(searchViewModel? obj)
        {
            var data = _adminRepo.adminDashboard();

            var dashData = new AdminDashboardViewModel
            {
                countRequestViewModel = data.countRequestViewModel,
            };

            return View(dashData);

        }

        [HttpPost]

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public async Task<IActionResult> PartialTable(int status, searchViewModel? obj, int pageNumber)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            int pageSize = 5;

            if (obj.Name != null || obj.reqType != 0 || obj.RegionId != 0)
            {

                if (status == (int)enumsFile.DashboardStatus.newStatus)
                {
                    var parseData = _adminRepo.newReq(obj);
                    return PartialView("_newRequestView", await PaginatedList<newReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == (int)enumsFile.DashboardStatus.pending)
                {
                    var parseData = _adminRepo.pendingReq(obj);
                    return PartialView("_PendingRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == (int)enumsFile.DashboardStatus.active)
                {
                    var parseData = _adminRepo.activeReq(obj);
                    return PartialView("_activeRequestView", await PaginatedList<activeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == (int)enumsFile.DashboardStatus.conclude)
                {
                    var parseData = _adminRepo.concludeReq(obj);
                    return PartialView("_concludeReqView", await PaginatedList<concludeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == (int)enumsFile.DashboardStatus.close)
                {
                    var parseData = _adminRepo.closeReq(obj);
                    return PartialView("_closeReqView", await PaginatedList<closeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else
                {
                    var parseData = _adminRepo.unpaidReq(obj);
                    return PartialView("_unpaidReqView", await PaginatedList<unpaidReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
            }

            if (status == (int)enumsFile.DashboardStatus.newStatus)
            {
                var parseData = _adminRepo.newReq();
                return PartialView("_newRequestView", await PaginatedList<newReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == (int)enumsFile.DashboardStatus.pending)
            {
                var parseData = _adminRepo.pendingReq();
                return PartialView("_PendingRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == (int)enumsFile.DashboardStatus.active)
            {
                var parseData = _adminRepo.activeReq();
                return PartialView("_activeRequestView", await PaginatedList<activeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == (int)enumsFile.DashboardStatus.conclude)
            {
                var parseData = _adminRepo.concludeReq();
                return PartialView("_concludeReqView", await PaginatedList<concludeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == (int)enumsFile.DashboardStatus.close)
            {
                var parseData = _adminRepo.closeReq();
                return PartialView("_closeReqView", await PaginatedList<closeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else
            {
                var parseData = _adminRepo.unpaidReq();
                return PartialView("_unpaidReqView", await PaginatedList<unpaidReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
        }


        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult CreateRequest()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = new PatientViewModel()
            {
                Regions = _requestRepo.Regions()
            };

            return View(data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult CreateRequest(PatientViewModel obj)
        {

            if (ModelState.IsValid)
            {
                _requestRepo.CreatePatientRequest(obj);

                _notyf.Success("Request Created");
                return RedirectToAction("Dashboard");
            }
            _notyf.Error("Something went wrong!!");
            return View();
        }


        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult SendEmailToPatient(string FirstName, string LastName, string Email, string PhoneNumber)
        {
            var subject = "Send your request";
            var body = "<a href='/HomeController/Index+'>HalloDoc</a>";
            _loginRepo.SendEmail(Email, subject, body, null);
            Task<bool> val = _sms.SendSmsAsync("+91" + PhoneNumber, body);

            _notyf.Success("Email sent.");
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public FileResult Export(int status, searchViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            byte[] excelBytes;

            if (status == (int)enumsFile.DashboardStatus.active)
            {
                IEnumerable<activeReqViewModel> data = _adminRepo.activeReq(obj).ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.pending)
            {
                IEnumerable<pendingReqViewModel> data = _adminRepo.pendingReq(obj).ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.conclude)
            {
                IEnumerable<concludeReqViewModel> data = _adminRepo.concludeReq(obj).ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.close)
            {
                IEnumerable<closeReqViewModel> data = _adminRepo.closeReq(obj).ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.unpaid)
            {
                IEnumerable<unpaidReqViewModel> data = _adminRepo.unpaidReq(obj).ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else
            {
                IEnumerable<newReqViewModel> data = _adminRepo.newReq(obj).ToList();
                excelBytes = _common.fileToExcel(data);
            }


            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sheet.xlsx");
        }

        public FileResult ExportAll(int status)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            byte[] excelBytes;

            if (status == (int)enumsFile.DashboardStatus.active)
            {
                IEnumerable<activeReqViewModel> data = _adminRepo.activeReq().ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.pending)
            {
                IEnumerable<pendingReqViewModel> data = _adminRepo.pendingReq().ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.conclude)
            {
                IEnumerable<concludeReqViewModel> data = _adminRepo.concludeReq().ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.close)
            {
                IEnumerable<closeReqViewModel> data = _adminRepo.closeReq().ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else if (status == (int)enumsFile.DashboardStatus.unpaid)
            {
                IEnumerable<unpaidReqViewModel> data = _adminRepo.unpaidReq().ToList();
                excelBytes = _common.fileToExcel(data);
            }
            else
            {
                IEnumerable<newReqViewModel> data = _adminRepo.newReq().ToList();
                excelBytes = _common.fileToExcel(data);
            }


            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sheet.xlsx");
        }

        //Request DTY Support
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult RequestSupport(string RequestNote)
        {
            var phy = _adminRepo.RequestSupportDTY();
            foreach (var item in phy)
            {
                _loginRepo.SendEmail(item.Email, "Request Support", RequestNote, null);
            }
            _notyf.Success("Mail send to Provider.");
            return RedirectToAction("Dashboard");
        }

        #endregion


        #region DashboardAction

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult ViewCase(int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var viewdata = _adminRepo.viewCase(reqClientId);

            return View(viewdata);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult ViewCase(viewCaseViewModel obj)
        {

            bool task = _adminRepo.viewCase(obj);

            if (task)
            {
                ViewBag.success = "updated successfully";
                return RedirectToAction("Dashboard", new { status = obj.status });
            }
            else
            {
                ViewBag.error = "Error Occured!!!!";
                return RedirectToAction("Dashboard");
            }

        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult ViewNote(int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            int adminId = _adminRepo.GetAdminId(AspId);
            var data = _adminRepo.ViewNoteGet(reqClientId);

            return View(data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult ViewNote(string adminNote, int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            if (adminNote != null)
            {
                int adminId = _adminRepo.GetAdminId(AspId);
                _adminRepo.ViewNotePost(reqClientId, adminNote, adminId);
                _notyf.Success("Updated");
            }
            else
            {
                _notyf.Error("Enter Note FIrst!!");
            }

            return RedirectToAction("ViewNote", new { reqClientId = reqClientId });
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult CancelCase(int CaseTag, string addNote, int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);


            if (CaseTag == 0 || string.IsNullOrEmpty(addNote))
            {
                //_notyf.Warning("Failed!!");
                return BadRequest(new { error = "Invalid data provided" });
            }
            else
            {


                _adminRepo.CancelCase(CaseTag, addNote, reqClientId, adminId);

                //_notyf.Success("Case Cancelled.");
                return Ok(new { success = true });
            }

        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult BlockCase(int reqClientId, string addNote)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);

            if (string.IsNullOrEmpty(addNote))
            {
                //_notyf.Warning("Failed!!");
                return BadRequest(new { error = "Invalid data provided" });
            }
            else
            {

                _adminRepo.BlockCase(reqClientId, addNote, adminId);
                //_notyf.Success("Case Blocked.");
                return Ok(new { success = true });
            }

        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public object FilterPhysician(int Region, int phyid)
        {
            return _adminRepo.FilterPhysician(Region, phyid);
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult AssignCase(int reqClientId, string addNote, int PhysicianSelect, string RegionSelect)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);

            if (PhysicianSelect == 0 || string.IsNullOrEmpty(RegionSelect))
            {
                _notyf.Warning("Failed!!");
                return BadRequest(new { error = "Invalid data provided" });
            }
            else
            {
                _adminRepo.AssignCase(reqClientId, addNote, PhysicianSelect, RegionSelect, adminId, AspId);

                _notyf.Success("Physician Assigned Successfully.");
                return Ok(new { success = true });
            }
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult ViewUpload(int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            int adminId = _adminRepo.GetAdminId(AspId);
            var data = _adminRepo.ViewUpload(reqClientId);
            return View(data);
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult UploadDocument(UploadFileViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);
            int id = _adminRepo.GetReqId(obj.reqId);
            if (obj.formFile != null && obj.formFile.Length > 0)
            {
                _loginRepo.uploadFile(obj.formFile, "RequestData\\" + obj.reqId, obj.formFile.FileName.ToString());
                var reqid = _adminRepo.ViewUploadFile(obj.formFile.FileName.ToString(), obj.reqId, adminId);
                _notyf.Success("File uploaded.");
                return RedirectToAction("ViewUpload", new { reqClientId = id });
            }
            else
            {
                _notyf.Error("Select File First.");
                return RedirectToAction("ViewUpload", new { reqClientId = id });
            }
        }

        public IActionResult SendFileToPatient(string[] files, int reqClientId)
        {

            _loginRepo.SendEmail(_adminRepo.GetPatientEmail(reqClientId), "Documents", "", files);
            return Ok();
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult DeleteFile(int reqClientId, string FileName)
        {
            _adminRepo.DeleteFile(reqClientId, FileName);
            _notyf.Success("File uploaded.");
            return Ok();
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult TransferCase(int reqClientId, string addNote, int PhysicianSelect, string RegionSelect)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);

            //_adminRepo.AssignCase(reqClientId, addNote, PhysicianSelect, RegionSelect, adminId, AspId);

            if (PhysicianSelect == 0 || string.IsNullOrEmpty(RegionSelect) || string.IsNullOrEmpty(addNote))
            {
                //_notyf.Warning("Failed!!");
                return BadRequest(new { error = "Invalid data provided" });
            }
            else
            {
                _adminRepo.AssignCase(reqClientId, addNote, PhysicianSelect, RegionSelect, adminId, AspId);

                //_notyf.Success("Request Transfered Successfully.");
                return Ok(new { success = true });
            }

        }


        [RoleAuth((int)enumsFile.adminRoles.SendOrder)]
        public IActionResult SendOrders(int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = _adminRepo.SendOrders(reqClientId);

            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.SendOrder)]
        public IActionResult SendOrders(SendOrderViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            if (ModelState.IsValid)
            {
                _adminRepo.SendOrders(obj, AspId);
                _notyf.Success("Ordered Successfully.");
                return RedirectToAction("SendOrders", new { reqClientId = obj.reqClientId });
            }
            else
            {
                var data = _adminRepo.SendOrders(obj.reqClientId);
                obj.Healthprofessionaltype = data.Healthprofessionaltype;
                _notyf.Error("Ordered Failed.");
                return View(obj);
            }
        }


        [RoleAuth((int)enumsFile.adminRoles.SendOrder)]
        public object FilterProfession(int ProfessionId)
        {
            return _adminRepo.FilterProfession(ProfessionId);
        }


        [RoleAuth((int)enumsFile.adminRoles.SendOrder)]
        public object ShowVendorDetail(int selectVendor)
        {
            return _adminRepo.ShowVendorDetail(selectVendor);
        }


        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public void ClearCase(int reqClientId)
        {
            _adminRepo.ClearCase(reqClientId);
        }


        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public void SendAgreement(int reqClientId, string email, string phone)
        {

            var callBackUrl = Url.Action("ReviewAgreement", "Home", new { reqClientId }, protocol: HttpContext.Request.Scheme, host: "localhost:44349");

            string subject = "Regarding Agreement";
            string body = "<a href=" + callBackUrl + ">Review</a>";

            _loginRepo.SendEmail(email, subject, body, null);
        }


        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult CloseCase(int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            int adminId = _adminRepo.GetAdminId(AspId);
            var data = _adminRepo.CloseCase(reqClientId);

            return View(data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult CloseCase(CloseCaseViewModel obj)
        {
            _adminRepo.CloseCase(obj);
            return RedirectToAction("CloseCase", new { reqClientId = obj.ReqClientId });
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult CloseToUnpaidCase(int reqClientId)
        {
            _adminRepo.CloseToUnpaidCase(reqClientId);
            return RedirectToAction("Dashboard", new { status = 13 });
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult EncounterForm(int reqClientId)
        {
            var obj = _adminRepo.Encounter(reqClientId);
            ViewBag.status = _adminRepo.GetStatus(reqClientId);
            return View(obj);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult EncounterForm(Encounter obj)
        {
            if (ModelState.IsValid)
            {
                _adminRepo.Encounter(obj);
                _notyf.Success("Updated");
            }
            else
            {
                _notyf.Error("Failed");
            }
            return View(obj);
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult ChatWithProvider(int reqclientid)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var data = _adminRepo.ChatWithProvider(reqclientid, _adminRepo.GetAdminId(AspId));
            return PartialView("_ChatView" ,data);
        }

        [RoleAuth((int)enumsFile.adminRoles.AdminDashboard)]
        public IActionResult ChatWithPatient(int reqclientid)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var data = _adminRepo.ChatWithPatient(reqclientid, _adminRepo.GetAdminId(AspId));
            return PartialView("_ChatView", data);
        }
        public void StoreChat(int reqClientId, int senderId, string message, int AccountTypeOfReceiver)
        {
            _adminRepo.StoreChat(reqClientId, senderId, message, AccountTypeOfReceiver);   
        }

        #endregion


        #region AdminProfile

        [RoleAuth((int)enumsFile.adminRoles.MyProfile)]
        public IActionResult MyProfile()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = _adminRepo.MyProfile(AspId);

            return View(data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.MyProfile)]
        public IActionResult MyProfile(Profile obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            string roleId = jwt.Claims.First(c => c.Type == "RoleId").Value;
            string Email = jwt.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            if (obj.Firstname != null && _loginRepo.isEmailAvailable(obj.Email) && _adminRepo.GetAdminEmail(obj.Adminid) != obj.Email)
            {
                _notyf.Error("Email is already registered.");
                return RedirectToAction("MyProfile");
            }

            if (ModelState.IsValid)
            {
                if (obj.Firstname != null)
                {

                    Response.Cookies.Delete("jwt");
                    var user2 = new LoggedUser
                    {
                        AspId = AspId,
                        FirstName = obj.Firstname,
                        LastName = obj.Lastname,
                        Email = Email,
                        Role = "Admin",
                        Roleid = roleId,
                    };
                    var jwtToken = _jwtService.GenerateJwtToken(user2);
                    Response.Cookies.Append("jwt", jwtToken);
                }   
                ViewBag.Data = obj.Firstname + " " + obj.Lastname;

                _adminRepo.MyProfile(obj, AspId);
                _notyf.Success("Updated");
            }
            else
            {
                _notyf.Error("Something went wrong");
            }

            return RedirectToAction("MyProfile");
        }

        //[RoleAuth((int)enumsFile.adminRoles.MyProfile)]
        //public void AdminRegionUpdate(List<CheckBoxData> selectedRegion, int adminId)
        //{
        //    _adminRepo.AdminRegionUpdate(selectedRegion, adminId);
        //}

        [RoleAuth((int)enumsFile.adminRoles.MyProfile)]
        public IActionResult ResetPass(string pass, int adminId)
        {
            if (pass == null)
            {
                _notyf.Warning(" Password can not be null");

                return RedirectToAction("MyProfile");
            }

            string hashPass = _loginRepo.GetHash(pass);

            _adminRepo.ResetAdminPass(hashPass, adminId);

            _notyf.Success("Successful Password Changed");

            return RedirectToAction("MyProfile", "AdminDashboard");
        }

        #endregion


        #region ProivderMenu    

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult Provider()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;


            var provider = _adminRepo.Provider();
            return View(provider);
        }

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public void StopNoty(int Physicianid)
        {
            _adminRepo.StopNoty(Physicianid);
        }

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult ProviderFilter(int RegionId)
        {
            ProviderViewModel data = _adminRepo.FilterProvider((int)RegionId);
            return PartialView("_ProviderTable", data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult ContactProvider(string Email, string note, string Mobile, int contactType)
        {
            var sub = "hey there";

            switch (contactType)
            {
                case 2:
                    _loginRepo.SendEmail(Email, sub, note, null);
                    break;
                case 1:
                    _sms.SendSmsAsync(Mobile, note);
                    break;
                case 3:
                    _loginRepo.SendEmail(Email, sub, note, null);
                    _sms.SendSmsAsync(Mobile, note);
                    break;
            }


            return RedirectToAction("Provider");
        }

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult EditProvider(int Physicianid)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var data = _adminRepo.EditProvider(Physicianid);
            return View(data);
        }
        [HttpPost]

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult EditProvider(EditProvider obj, int formid)
        {
            switch (formid)
            {
                case 1:
                    _adminRepo.ProviderAccountEdit(obj);
                    break;
                case 0:
                    _adminRepo.ProviderInfoEdit(obj);
                    break;
                case 3:
                    _adminRepo.ProviderMailingInfoEdit(obj);
                    break;
                case 4:
                    _adminRepo.ProviderProfileEdit(obj);
                    if (obj.PhySign != null)
                    {
                        _loginRepo.uploadFile(obj.PhySign, "ProviderData\\" + obj.Physicianid, obj.PhySign.FileName.ToString());
                    }
                    if (obj.PhyPhoto != null)
                    {
                        _loginRepo.uploadFile(obj.PhyPhoto, "ProviderData\\" + obj.Physicianid, obj.PhyPhoto.FileName.ToString());
                    }
                    break;
                case 5:
                    if (obj.Isagreementdoc)
                    {
                        _loginRepo.uploadFile(obj.agreementdoc, "ProviderData\\" + obj.Physicianid, obj.agreementdoc.FileName.ToString());
                        _adminRepo.UploadProviderFile(obj.Physicianid, obj.agreementdoc.FileName.ToString(), 1);
                    }
                    if (obj.Isbackgrounddoc)
                    {
                        _loginRepo.uploadFile(obj.backgrounddoc, "ProviderData\\" + obj.Physicianid, obj.backgrounddoc.FileName.ToString());
                        _adminRepo.UploadProviderFile(obj.Physicianid, obj.backgrounddoc.FileName.ToString(), 2);
                    }
                    if (obj.Islicensedoc)
                    {
                        _loginRepo.uploadFile(obj.licensedoc, "ProviderData\\" + obj.Physicianid, obj.licensedoc.FileName.ToString());
                        _adminRepo.UploadProviderFile(obj.Physicianid, obj.licensedoc.FileName.ToString(), 3);
                    }
                    if (obj.Isnondisclosuredoc)
                    {
                        _loginRepo.uploadFile(obj.nondisclosuredoc, "ProviderData\\" + obj.Physicianid, obj.nondisclosuredoc.FileName.ToString());
                        _adminRepo.UploadProviderFile(obj.Physicianid, obj.nondisclosuredoc.FileName.ToString(), 4);
                    }
                    if (obj.Istrainingdoc)
                    {
                        _loginRepo.uploadFile(obj.trainingdoc, "ProviderData\\" + obj.Physicianid, obj.trainingdoc.FileName.ToString());
                        _adminRepo.UploadProviderFile(obj.Physicianid, obj.trainingdoc.FileName.ToString(), 5);
                    }
                    break;
            }

            return RedirectToAction("EditProvider", new { Physicianid = obj.Physicianid });
        }


        //[RoleAuth((int)enumsFile.adminRoles.Provider)]
        //public void PhysicianRegionUpdate(List<CheckBoxData> selectedRegion, int Physicianid)
        //{
        //    _adminRepo.PhysicianRegionUpdate(selectedRegion, Physicianid);
        //}

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult ResetPhysicianPass(string pass, int Physicianid)
        {
            if (pass == null)
            {
                _notyf.Warning(" Password can not be null");

                return RedirectToAction("MyProfile");
            }

            string hashPass = _loginRepo.GetHash(pass);


            _adminRepo.ResetPhysicianPass(hashPass, Physicianid);
            _notyf.Success("Successful Password Changed");

            return RedirectToAction("EditProvider", new { Physicianid = Physicianid });
        }


        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult DeletePhysician(int Physicianid)
        {
            _adminRepo.DeletePhysician(Physicianid);

            return RedirectToAction("Provider", "AdminDashboard");
        }


        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult CreateProvider()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = _adminRepo.CreateProvider();
            return View(data);
        }

        [HttpPost]

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult CreateProvider(EditProvider obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            if (_loginRepo.isEmailAvailable(obj.Email))
            {
                _notyf.Error("Email Already Registered");
                var data = _adminRepo.CreateProvider();

                obj.Region = data.Region;
                obj.Role = data.Role;

                return View(obj);
            }

            if (ModelState.IsValid)
            {

                var pass = _loginRepo.GetHash(obj.Password);
                var phyId = _adminRepo.CreateProvider(obj, pass, AspId);


                if (obj.PhySign != null)
                {
                    _loginRepo.uploadFile(obj.PhySign, "ProviderData\\" + phyId, obj.PhySign.FileName.ToString());
                }
                if (obj.PhyPhoto != null)
                {
                    _loginRepo.uploadFile(obj.PhyPhoto, "ProviderData\\" + phyId, obj.PhyPhoto.FileName.ToString());
                }
                if (obj.Isagreementdoc)
                {
                    _loginRepo.uploadFile(obj.agreementdoc, "ProviderData\\" + phyId, obj.agreementdoc.FileName.ToString());
                    _adminRepo.UploadProviderFile(phyId, obj.agreementdoc.FileName.ToString(), 1);
                }
                if (obj.Isbackgrounddoc)
                {
                    _loginRepo.uploadFile(obj.backgrounddoc, "ProviderData\\" + phyId, obj.backgrounddoc.FileName.ToString());
                    _adminRepo.UploadProviderFile(phyId, obj.backgrounddoc.FileName.ToString(), 2);
                }
                if (obj.Islicensedoc)
                {
                    _loginRepo.uploadFile(obj.licensedoc, "ProviderData\\" + phyId, obj.licensedoc.FileName.ToString());
                    _adminRepo.UploadProviderFile(phyId, obj.licensedoc.FileName.ToString(), 3);
                }
                if (obj.Isnondisclosuredoc)
                {
                    _loginRepo.uploadFile(obj.nondisclosuredoc, "ProviderData\\" + phyId, obj.nondisclosuredoc.FileName.ToString());
                    _adminRepo.UploadProviderFile(phyId, obj.nondisclosuredoc.FileName.ToString(), 4);
                }
                if (obj.Istrainingdoc)
                {
                    _loginRepo.uploadFile(obj.trainingdoc, "ProviderData\\" + phyId, obj.trainingdoc.FileName.ToString());
                    _adminRepo.UploadProviderFile(phyId, obj.trainingdoc.FileName.ToString(), 5);
                }
                return RedirectToAction("CreateProvider");
            }
            else
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());

                return Json(new { success = false, errors = errors });
            }
        }

        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult PayRate(int id)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var data = _adminRepo.payrateCategories(id);
            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Provider)]
        public IActionResult PayRate(int categoryId, int phyId, int PayRate)
        {
            _adminRepo.PayRate(phyId, categoryId, PayRate);
            var data = _adminRepo.payrateCategories(phyId);
            return View(data);
        }

        #endregion


        #region AccesMenu

        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult Access()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = _adminRepo.CreateRole();
            return View(data);
        }


        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult CreateRole()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = new CreateRole
            {
                Menu = _adminRepo.Menus(),
                accountTypes = _adminRepo.AccountType()
            };

            return View(data);
        }


        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IEnumerable<Menu> PageListFilter(int id)
        {
            return _adminRepo.PageListFilter(id);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult CreateRole(string selectedPage, CreateRole obj, int AccountType)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            var PageList = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedPage);
            if (ModelState.IsValid)
            {
                _adminRepo.CreateRole(PageList, AspId, AccountType, obj.Name);
                _notyf.Success("Role Created");
                return RedirectToAction("Access");
            }
            else
            {
                _notyf.Error("Failed");
                return View();
            }

        }
        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult EditRole(int id)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = _adminRepo.EditRole(id);
            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult EditRole(string selectedPage, CreateRole obj, int AccountType)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            var PageList = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedPage);

            _adminRepo.EditRole(PageList, AspId, AccountType, obj);

            return RedirectToAction("Access");
        }
        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult DeleteRole(int id)
        {
            _adminRepo.DeleteRole(id);
            return RedirectToAction("Access");
        }
        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult CreateAdmin()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var data = _adminRepo.CreateAdmin();

            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult CreateAdmin(CreateAdminViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            if (obj.Password == null)
            {
                _notyf.Error("Enter Password");
                var data = _adminRepo.CreateAdmin();
                obj.Regions = data.Regions;
                obj.Rolemenus = data.Rolemenus;
                return View(obj);
            }
            if (_loginRepo.isEmailAvailable(obj.Email))
            {
                _notyf.Error("Email already Registered.");
                var data = _adminRepo.CreateAdmin();
                obj.Regions = data.Regions;
                obj.Rolemenus = data.Rolemenus;
                return View(obj);
            }

            var pass = _loginRepo.GetHash(obj.Password);
            _adminRepo.CreateAdmin(obj, pass, AspId);
            return RedirectToAction("CreateAdmin");
        }

        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult EditAdmin(int id)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            var data = _adminRepo.EditAdmin(id);
            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult EditAdmin(CreateAdminViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            if (_loginRepo.isEmailAvailable(obj.Email) && _adminRepo.GetAdminEmail(obj.Adminid) != obj.Email)
            {
                _notyf.Error("Email is already registered.");
                var data = _adminRepo.EditAdmin(obj.Adminid);
                obj.CheckedRegion = data.CheckedRegion;
                obj.Rolemenus = data.Rolemenus;
                obj.Regions = data.Regions;
                return View(obj);
            }

            _adminRepo.EditAdmin(obj, AspId);
            return RedirectToAction("EditAdmin", obj.Adminid);
        }

        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public IActionResult UserAccess()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

            var data = _adminRepo.UserAccess();
            return View(data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Accounts)]
        public async Task<IActionResult> UserAccessTable(int accountType, int RoleId, int pageNumber)
        {
            var data = _adminRepo.UserAccessTables(accountType, RoleId);

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 5;

            return PartialView("_UserAccessTable", await PaginatedList<UserAccessTable>.CreateAsync(data, pageNumber, pageSize));
        }


        #endregion
    }
}
