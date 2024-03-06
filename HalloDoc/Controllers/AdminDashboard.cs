    using Azure.Core;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
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
            var dashData = new AdminDashboardViewModel
            {
                countRequestViewModel = data.countRequestViewModel,
                Casetag =data.Casetag,
                Region =data.Region,
            };
            return View(dashData);
        }
        [HttpPost]
        public IActionResult Dashboard(searchViewModel? obj)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
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
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");

            var data = _adminRepo.adminDashboard();

            if (obj.Name != null || obj.reqType != 0 || obj.RegionId!=0)
            {
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
                var parseData = data.newReqViewModel;
                return PartialView("_newRequestView", parseData);
            }
            else if (status == 2)
            {
                var parseData = data.pendingReqViewModel;
                return PartialView("_PendingRequestView", parseData);
            }
            else if (status == 8)
            {
                var parseData = data.activeReqViewModels;
                return PartialView("_activeRequestView", parseData);
            }
            else if (status == 4)
            {
                var parseData = data.concludeReqViewModel;
                return PartialView("_concludeReqView", parseData);
            }
            else if (status == 5)
            {
                var parseData = data.closeReqViewModels;
                return PartialView("_closeReqView", parseData);
            }
            else
            {
                var parseData = data.unpaidReqViewModels;
                return PartialView("_unpaidReqView", parseData);
            }
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

            if (task)
            {
                ViewBag.success = "updated successfully";
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.error = "Error Occured!!!!";
                return RedirectToAction("Dashboard");
            }

        }

        public IActionResult ViewNote(int reqClientId)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            int adminId = ViewBag.AdminId;
            var data = _adminRepo.ViewNoteGet(reqClientId);
            
            return View(data);
        }

        [HttpPost]
        public IActionResult ViewNote(string adminNote, int reqClientId)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            int adminId = ViewBag.AdminId;
            _adminRepo.ViewNotePost(reqClientId, adminNote, adminId);

            return RedirectToAction("ViewNote", new { reqClientId = reqClientId });
        }

        [HttpPost]
        public IActionResult CancelCase(int CaseTag, string addNote, int reqClientId)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            int adminId = ViewBag.AdminId;

            _adminRepo.CancelCase(CaseTag, addNote, reqClientId, adminId);

            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        public IActionResult BlockCase(int reqClientId, string addNote)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            int adminId = ViewBag.AdminId;

            _adminRepo.BlockCase(reqClientId,addNote, adminId);

            return RedirectToAction("Dashboard");
        }

        public object FilterPhysician(int Region)
        {
            return _adminRepo.FilterPhysician(Region);
        }

        public IActionResult AssignCase(int reqClientId, string addNote, int PhysicianSelect, string RegionSelect)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            int adminId = ViewBag.AdminId;
            _adminRepo.AssignCase(reqClientId, addNote, PhysicianSelect, RegionSelect, adminId);

            return RedirectToAction("Dashboard");
        }

        public IActionResult ViewUpload(int reqClientId)
        {
            ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            int adminId = ViewBag.AdminId;
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

        public IActionResult SendOrders()
        {
            return View();
        }
        
    }
}
