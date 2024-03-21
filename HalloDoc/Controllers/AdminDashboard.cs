using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            _adminRepo = adminRepo;
            _db = db;
            _jwtService = jwtService;
            _notyf = notyf;
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

            var searchedData = _adminRepo.searchPatient(obj, data);

            var dashData = new AdminDashboardViewModel
            {
                countRequestViewModel = searchedData.countRequestViewModel,
            };

            return View(dashData);

        }

        [HttpPost]
        public IActionResult PartialTable(int status, searchViewModel? obj)
        {
            var data = _adminRepo.adminDashboard();

            if (obj.Name != null || obj.reqType != 0 || obj.RegionId != 0)
            {
                data.newReqViewModel = _adminRepo.newReq();
                data.pendingReqViewModel = _adminRepo.pendingReq();
                data.unpaidReqViewModels = _adminRepo.unpaidReq();
                data.activeReqViewModels = _adminRepo.activeReq();
                data.concludeReqViewModel = _adminRepo.concludeReq();
                data.closeReqViewModels = _adminRepo.closeReq();
                var searchData = _adminRepo.searchPatient(obj, data);
                if (status == 1)
                {
                    var parseData = searchData.newReqViewModel;
                    return PartialView("_newRequestView", parseData);
                }
                else if (status == 2)
                {
                    var parseData = searchData.pendingReqViewModel;
                    return PartialView("_PendingRequestView", parseData);
                }
                else if (status == 8)
                {
                    var parseData = searchData.activeReqViewModels;
                    return PartialView("_activeRequestView", parseData);
                }
                else if (status == 4)
                {
                    var parseData = searchData.concludeReqViewModel;
                    return PartialView("_concludeReqView", parseData);
                }
                else if (status == 5)
                {
                    var parseData = searchData.closeReqViewModels;
                    return PartialView("_closeReqView", parseData);
                }
                else
                {
                    var parseData = searchData.unpaidReqViewModels;
                    return PartialView("_unpaidReqView", parseData);
                }
            }

            if (status == 1)
            {
                var parseData = _adminRepo.newReq();
                return PartialView("_newRequestView", parseData);
            }
            else if (status == 2)
            {
                var parseData = _adminRepo.pendingReq();
                return PartialView("_PendingRequestView", parseData);
            }
            else if (status == 8)
            {
                var parseData = _adminRepo.activeReq();
                return PartialView("_activeRequestView", parseData);
            }
            else if (status == 4)
            {
                var parseData = _adminRepo.concludeReq();
                return PartialView("_concludeReqView", parseData);
            }
            else if (status == 5)
            {
                var parseData = _adminRepo.closeReq();
                return PartialView("_closeReqView", parseData);
            }
            else
            {
                var parseData = _adminRepo.unpaidReq();
                return PartialView("_unpaidReqView", parseData);
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
            _adminRepo.AssignCase(reqClientId, addNote, PhysicianSelect, RegionSelect, adminId, AspId);

            return RedirectToAction("Dashboard");
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
                        _loginRepo.uploadFile(obj.PhySign, "ProviderData", obj.PhySign.FileName.ToString());
                    }
                    if (obj.PhyPhoto != null)
                    {
                        _loginRepo.uploadFile(obj.PhyPhoto, "ProviderData", obj.PhyPhoto.FileName.ToString());
                    }
                    break;
                case 5:
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


        public IActionResult Access()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;


            return View();
        }

        public IActionResult CreateRole()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.AdminName = fname + "_" + lname;
            return View();
        }
    }
}
