using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class AdminDashboard : Controller
    {
        private readonly IAdminDashboardRepository _adminRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly ILoginRepository _loginRepo;
        private readonly ApplicationDbContext _db;
        private readonly IJwtService _jwtService;
        private readonly INotyfService _notyf;
        private readonly ISMSSender _sms;

        public AdminDashboard(IAdminDashboardRepository adminRepo, ApplicationDbContext db, IJwtService jwtService, INotyfService notyf, ILoginRepository loginRepo, IRequestRepository requestRepo, ISMSSender sms)
        {
            _notyf = notyf;
            _adminRepo = adminRepo;
            _db = db;
            _jwtService = jwtService;
            _loginRepo = loginRepo;
            _requestRepo = requestRepo;
            _sms = sms;
        }

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
        public async Task<IActionResult> PartialTable(int status, searchViewModel? obj, int pageNumber)
        {
            var data = _adminRepo.adminDashboard();
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            int pageSize = 2;

            if (obj.Name != null || obj.reqType != 0 || obj.RegionId != 0)
            {

                if (status == 1)
                {
                    var parseData = _adminRepo.newReq(obj);
                    //return PartialView("_newRequestView", parseData);
                    return PartialView("_newRequestView", await PaginatedList<newReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == 2)
                {
                    var parseData = _adminRepo.pendingReq(obj);
                    //return PartialView("_PendingRequestView", parseData);
                    return PartialView("_PendingRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == 8)
                {
                    var parseData = _adminRepo.activeReq(obj);
                    //return PartialView("_activeRequestView", parseData);
                    return PartialView("_activeRequestView", await PaginatedList<activeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == 4)
                {
                    var parseData = _adminRepo.concludeReq(obj);
                    //return PartialView("_concludeReqView", parseData);
                    return PartialView("_concludeReqView", await PaginatedList<concludeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else if (status == 5)
                {
                    var parseData = _adminRepo.closeReq(obj);
                    //return PartialView("_closeReqView", parseData);
                    return PartialView("_closeReqView", await PaginatedList<closeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
                else
                {
                    var parseData = _adminRepo.unpaidReq(obj);
                    //return PartialView("_unpaidReqView", parseData);
                    return PartialView("_unpaidReqView", await PaginatedList<unpaidReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
                }
            }

            if (status == 1)
            {
                var parseData = _adminRepo.newReq();
                //return PartialView("_newRequestView", parseData);
                return PartialView("_newRequestView", await PaginatedList<newReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == 2)
            {
                var parseData = _adminRepo.pendingReq();
                return PartialView("_PendingRequestView", await PaginatedList<pendingReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == 8)
            {
                var parseData = _adminRepo.activeReq();
                return PartialView("_activeRequestView", await PaginatedList<activeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == 4)
            {
                var parseData = _adminRepo.concludeReq();
                return PartialView("_concludeReqView", await PaginatedList<concludeReqViewModel>.CreateAsync(parseData, pageNumber, pageSize));
            }
            else if (status == 5)
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

        public IActionResult CreateRequest()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;

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

        public IActionResult SendEmailToPatient(string FirstName, string LastName, string Email, string PhoneNumber)
        {
            var subject = "Send your request";
            var body = "<a href='/HomeController/Index+'>HalloDoc</a>";
            _loginRepo.SendEmail(Email, subject, body);
            Task<bool> val = _sms.SendSmsAsync("+91" + PhoneNumber, body);

            return RedirectToAction("Dashboard");
        }

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
        public IActionResult ViewNote(string adminNote, int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            int adminId = _adminRepo.GetAdminId(AspId);
            _adminRepo.ViewNotePost(reqClientId, adminNote, adminId);

            return RedirectToAction("ViewNote", new { reqClientId = reqClientId });
        }

        [HttpPost]
        public IActionResult CancelCase(int CaseTag, string addNote, int reqClientId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);

            _adminRepo.CancelCase(CaseTag, addNote, reqClientId, adminId);

            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        public IActionResult BlockCase(int reqClientId, string addNote)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);

            _adminRepo.BlockCase(reqClientId, addNote, adminId);

            return RedirectToAction("Dashboard");
        }

        public object FilterPhysician(int Region)
        {
            return _adminRepo.FilterPhysician(Region);
        }

        public IActionResult AssignCase(int reqClientId, string addNote, int PhysicianSelect, string RegionSelect)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);

            if (PhysicianSelect == 0 || string.IsNullOrEmpty(RegionSelect))
            {
                _notyf.Warning("Login Failed");
                return BadRequest(new { error = "Invalid data provided" });
            }

            _adminRepo.AssignCase(reqClientId, addNote, PhysicianSelect, RegionSelect, adminId, AspId);

            //return RedirectToAction("Dashboard");
            _notyf.Success("Physician Assigned Successfully.");
            return Ok(new { success = true });
        }

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

        public IActionResult UploadDocument(UploadFileViewModel obj)
        {
            var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == obj.reqId).FirstOrDefault();

            if (obj.formFile != null && obj.formFile.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(obj.formFile.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    obj.formFile.CopyTo(stream);
                }
                Requestwisefile requestwisefile = new Requestwisefile
                {
                    Filename = fileName,
                    Requestid = (int)reqClientRow.Requestid,
                    Createddate = DateTime.Now
                };

                _db.Requestwisefiles.Add(requestwisefile);
                _db.SaveChanges();
            }
            return RedirectToAction("ViewUpload", new { reqClientId = obj.reqId });
        }
        public void DeleteFile(int reqClientId, string FileName)
        {
            _adminRepo.DeleteFile(reqClientId, FileName);

            //return RedirectToAction("ViewUpload", new { reqClientId = ReqClientId}); 
        }

        public void TransferCase(int reqClientId, string addNote, int PhysicianSelect, string RegionSelect)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            int adminId = _adminRepo.GetAdminId(AspId);

            _adminRepo.AssignCase(reqClientId, addNote, PhysicianSelect, RegionSelect, adminId, AspId);
        }

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
        public IActionResult SendOrders(SendOrderViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            _adminRepo.SendOrders(obj, AspId);

            return RedirectToAction("SendOrders", new { reqClientId = obj.reqClientId });
        }


        public object FilterProfession(int ProfessionId)
        {
            return _adminRepo.FilterProfession(ProfessionId);
        }

        public object ShowVendorDetail(int selectVendor)
        {
            return _adminRepo.ShowVendorDetail(selectVendor);
        }

        public void ClearCase(int reqClientId)
        {
            _adminRepo.ClearCase(reqClientId);
        }

        public void SendAgreement(int reqClientId, string email, string phone)
        {

            var callBackUrl = Url.Action("ReviewAgreement", "Home", new { reqClientId }, protocol: HttpContext.Request.Scheme, host: "localhost:44349");

            string subject = "Regarding Agreement";
            string body = "<a href=" + callBackUrl + ">Review</a>";

            _loginRepo.SendEmail(email, subject, body);
        }

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
        public IActionResult CloseCase(CloseCaseViewModel obj)
        {
            _adminRepo.CloseCase(obj);
            return RedirectToAction("CloseCase", new { reqClientId = obj.ReqClientId });
        }

        public IActionResult CloseToUnpaidCase(int reqClientId)
        {
            _adminRepo.CloseToUnpaidCase(reqClientId);
            return RedirectToAction("Dashboard", new { status = 13 });
        }

        public IActionResult Encounter(int reqClientId, string option)
        {

            var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqClientRow.Requestid).FirstOrDefault();
            if (option == "Consult")
            {
                reqRow.Status = 4;
            }
            else
            {
                reqRow.Status = 15;
            }
            _db.Requests.Update(reqRow);
            _db.SaveChanges();

            return RedirectToAction("Dashboard", new { status = reqRow.Status });
        }

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
        public IActionResult MyProfile(Profile obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            _adminRepo.MyProfile(obj, AspId);

            return RedirectToAction("MyProfile");
        }

        public void AdminRegionUpdate(List<CheckBoxData> selectedRegion, int adminId)
        {
            _adminRepo.AdminRegionUpdate(selectedRegion, adminId);
        }

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

        public IActionResult EncounterForm(int reqClientId)
        {
            var obj = _adminRepo.Encounter(reqClientId);
            ViewBag.status = _adminRepo.GetStatus(reqClientId);
            return View(obj);
        }
        [HttpPost]
        public IActionResult EncounterForm(Encounter obj)
        {

            _adminRepo.Encounter(obj);
            return View(obj);
        }


        [HttpPost]
        public FileResult Export(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }

        public FileResult ExportAll(int status)
        {
            byte[] excelBytes;

            if (status == 8)
            {
                IEnumerable<activeReqViewModel> data = _adminRepo.activeReq().ToList();
                excelBytes = fileToExcel(data);
            }
            else if (status == 2)
            {
                IEnumerable<pendingReqViewModel> data = _adminRepo.pendingReq().ToList();
                excelBytes = fileToExcel(data);
            }
            else if (status == 4)
            {
                IEnumerable<concludeReqViewModel> data = _adminRepo.concludeReq().ToList();
                excelBytes = fileToExcel(data);
            }
            else if (status == 5)
            {
                IEnumerable<closeReqViewModel> data = _adminRepo.closeReq().ToList();
                excelBytes = fileToExcel(data);
            }
            else if (status == 13)
            {
                IEnumerable<unpaidReqViewModel> data = _adminRepo.unpaidReq().ToList();
                excelBytes = fileToExcel(data);
            }
            else
            {
                IEnumerable<newReqViewModel> data = _adminRepo.newReq().ToList();
                excelBytes = fileToExcel(data);
            }


            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sheet.xlsx");
        }

        public byte[] fileToExcel<T>(IEnumerable<T> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");

                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                }
                int row = 2;

                foreach (var item in data)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[row, i + 1].Value = properties[i].GetValue(item);
                    }
                    row++;
                }

                byte[] excelBytes = package.GetAsByteArray();

                return excelBytes;
            }

        }

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

        public void StopNoty(int Physicianid)
        {
            _adminRepo.StopNoty(Physicianid);
        }

        public IActionResult ProviderFilter(int RegionId)
        {
            ProviderViewModel data = _adminRepo.FilterProvider((int)RegionId);
            return PartialView("_ProviderTable", data);
        }

        public IActionResult ContactProvider(string Email, string note, string Mobile, int contactType)
        {
            var sub = "hey there";

            switch (contactType)
            {
                case 2:
                    _loginRepo.SendEmail(Email, sub, note);
                    break;
                case 1:
                    _sms.SendSmsAsync(Mobile, note);
                    break;
                case 3:
                    _loginRepo.SendEmail(Email, sub, note);
                    _sms.SendSmsAsync(Mobile, note);
                    break;
            }


            return RedirectToAction("Provider");
        }

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
        public IActionResult EditProvider(EditProvider obj, int formid)
        {
            switch (formid)
            {
                case 1:
                    _adminRepo.ProviderAccountEdit(obj);
                    break;
                case 2:
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

        public void PhysicianRegionUpdate(List<CheckBoxData> selectedRegion, int Physicianid)
        {
            _adminRepo.PhysicianRegionUpdate(selectedRegion, Physicianid);
        }

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

        public IActionResult DeletePhysician(int Physicianid)
        {
            _adminRepo.DeletePhysician(Physicianid);

            return RedirectToAction("Provider", "AdminDashboard");
        }

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
        public IActionResult CreateProvider(string selectedRegion, EditProvider obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            var Region = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedRegion);

            var pass = _loginRepo.GetHash(obj.Password);
            var phyId = _adminRepo.CreateProvider(obj, pass, AspId, Region);

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
                Menu = _db.Menus,
                accountTypes = _db.AccountTypes
            };

            return View(data);
        }

        public IEnumerable<Menu> PageListFilter(int id)
        {
            return _adminRepo.PageListFilter(id);
        }

        [HttpPost]
        public IActionResult CreateRole(string selectedPage, CreateRole obj, int AccountType)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            var PageList = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedPage);

            _adminRepo.CreateRole(PageList, AspId, AccountType, obj.Name);

            return RedirectToAction("Access");
        }

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
        public IActionResult EditRole(string selectedPage, CreateRole obj, int AccountType)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            var PageList = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedPage);

            _adminRepo.EditRole(PageList, AspId, AccountType, obj);

            return RedirectToAction("Access");
        }

        public IActionResult DeleteRole(int id)
        {
            _adminRepo.DeleteRole(id);
            return RedirectToAction("Access");
        }

        public IActionResult CreateAdmin()
        {
            var data = _adminRepo.CreateAdmin();

            return View(data);
        }
        [HttpPost]
        public IActionResult CreateAdmin(string selectedRegion, CreateAdminViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            var Region = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedRegion);

            var pass = _loginRepo.GetHash(obj.Password);
            _adminRepo.CreateAdmin(obj, pass, AspId, Region);
            return RedirectToAction("CreateAdmin");
        }

    }
}
