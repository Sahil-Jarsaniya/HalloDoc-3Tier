using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PatientController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Dashboard(String AspId)
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            }
            else
            {
                return RedirectToAction("login");
            }

            var patientAspId = _db.Users.Where(x => x.Aspnetuserid == AspId).FirstOrDefault();
            var userId = patientAspId.Userid;

            var requestData = from t1 in _db.Requests
                              join t3 in _db.RequestStatuses on t1.Status equals t3.StatusId
                              join t2 in _db.Requestwisefiles
                              on t1.Requestid equals t2.Requestid into files
                              from t2 in files.DefaultIfEmpty()
                              where t1.Userid == userId
                              select new PatientDashboardViewModel
                              {
                                  RequestId = t1.Requestid,
                                  Createddate = t1.Createddate,
                                  Status = t3.Status,
                                  Filename = t2 != null ? t2.Filename : null
                              };

            var userdata = new ProfileEditViewModel
            {
                UserId = patientAspId.Userid,
                Firstname = patientAspId.Firstname,
                Lastname = patientAspId.Lastname,
                Email = patientAspId.Email,
                Phonenumber = patientAspId.Mobile,
                Strmonth = patientAspId.Strmonth,
                Street = patientAspId.Street,
                City = patientAspId.City,
                State = patientAspId.State,
                Zipcode = patientAspId.Zipcode
            };

            var data = new DashboardViewModel
            {
                PatientDashboardViewModel = requestData,
                ProfileEditViewModel = userdata
            };

            return View(data);
        }

        public IActionResult Document(int reqId)
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            }
            else
            {
                return RedirectToAction("login");
            }
            var aspid = from t1 in _db.Requests
                        join t2 in _db.Users on t1.Userid equals t2.Userid
                        where t1.Requestid == reqId
                        select t2.Aspnetuserid;
            var requestData = from t1 in _db.Requests
                              join t2 in _db.Requestwisefiles
                              on t1.Requestid equals t2.Requestid into files
                              from t2 in files.DefaultIfEmpty()
                              where t1.Requestid == reqId
                              select new PatientDocumentViewModel
                              {
                                  RequestId = t1.Requestid,
                                  createdate = t1.Createddate,
                                  Filename = t2 != null ? t2.Filename : null
                              };

            return View(requestData);
        }

        [HttpPost]
        public IActionResult Document(int? reqId)
        {
            var aspId = from t1 in _db.Requests
                        join t2 in _db.Users on t1.Userid equals t2.Userid
                        join t3 in _db.AspNetUsers on t2.Aspnetuserid equals t3.Id
                        where t1.Requestid == reqId
                        select t3.Id;
            return RedirectToAction("Dashboard", "Patient", new { AspId = aspId });
        }

        [HttpPost]
        public IActionResult Profile(DashboardViewModel obj)
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            }
            else
            {
                return RedirectToAction("login");
            }

            var aspId = from t1 in _db.AspNetUsers
                        join t2 in _db.Users on t1.Id equals t2.Aspnetuserid
                        where t2.Userid == obj.ProfileEditViewModel.UserId
                        select t1.Id;

            var existUser = _db.Users.FirstOrDefault(x => x.Userid == obj.ProfileEditViewModel.UserId);

            existUser.Firstname = obj.ProfileEditViewModel.Firstname;
            existUser.Lastname = obj.ProfileEditViewModel.Lastname;
            existUser.Email = obj.ProfileEditViewModel.Email;
            existUser.Mobile = obj.ProfileEditViewModel.Phonenumber;
            existUser.Street = obj.ProfileEditViewModel.Street;
            existUser.City = obj.ProfileEditViewModel.City;
            existUser.State = obj.ProfileEditViewModel.State;
            existUser.Zipcode = obj.ProfileEditViewModel.Zipcode;
            existUser.Modifieddate = DateTime.Now;
            _db.Users.Update(existUser);
            _db.SaveChanges();

            var existAsp = _db.AspNetUsers.FirstOrDefault(x => x.Id == existUser.Aspnetuserid);

            existAsp.Email = obj.ProfileEditViewModel.Email;
            existAsp.PhoneNumber = obj.ProfileEditViewModel.Phonenumber;
            existAsp.ModifiedDate = DateTime.UtcNow;

            _db.AspNetUsers.Update(existAsp);
            _db.SaveChanges();

            

            return RedirectToAction("Dashboard", new { AspId = aspId });
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        public async Task<IActionResult> Download(String filename)
        {
            if (filename == null)
                return Content("filename is not availble");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        // Get mime types
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
    {
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"},
        {".svg", "image/svg"}
    };
        }
    }
}
