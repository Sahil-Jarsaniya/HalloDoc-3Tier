using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Xml.Serialization;

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

        public AdminDashboard(IAdminDashboardRepository adminRepo, ApplicationDbContext db, IJwtService jwtService, INotyfService notyf, ILoginRepository loginRepo, IRequestRepository requestRepo)
        {
            _adminRepo = adminRepo;
            _db = db;
            _jwtService = jwtService;
            _notyf = notyf;
            _loginRepo = loginRepo;
            _requestRepo = requestRepo;
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
            ViewBag.Data = fname + " " + lname;

            return View();
        }

        [HttpPost]
        public IActionResult CreateRequest(FamilyViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            ViewBag.Data = fname + " " + lname;
            if (ModelState.IsValid)
            {
                _requestRepo.CreateFamilyfriendRequest(obj);

                return RedirectToAction("Dashboard");
            }
            return View();
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
            object data;
            XmlSerializer serializer;
            if (status == 8)
            {
                 data = _adminRepo.activeReq().ToList();
             serializer = new XmlSerializer(typeof(List<activeReqViewModel>));
           
            }
            else if (status == 2)
            {
                 data = _adminRepo.pendingReq().ToList();
                serializer = new XmlSerializer(typeof(List<pendingReqViewModel>));
            }
            else if (status == 4)
            {
                 data = _adminRepo.concludeReq().ToList();
                serializer = new XmlSerializer(typeof(List<concludeReqViewModel>));
            }
            else if (status == 5)
            {
                 data = _adminRepo.closeReq().ToList(); 
                serializer = new XmlSerializer(typeof(List<closeReqViewModel>));
            }
            else if (status == 13)
            {
                 data = _adminRepo.unpaidReq().ToList();
                serializer = new XmlSerializer(typeof(List<unpaidReqViewModel>));
            }
            else
            {
                 data = _adminRepo.newReq().ToList(); 
                serializer = new XmlSerializer(typeof(List<newReqViewModel>));
               
            }


            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, data);
                stream.Position = 0;


                byte[] xmlBytes = stream.ToArray();

                return File(stream, "application/xml", "Object.xml");
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
    }
}
