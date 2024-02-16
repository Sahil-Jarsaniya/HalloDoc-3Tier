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

            var reqData = (from r in _db.Requests
                                    where r.Userid == userId
                                    join s in _db.RequestStatuses on r.Status equals s.StatusId
                                    select new PatientDashboardViewModel
                                    {
                                        RequestId = r.Requestid,
                                        fileCount = 1,
                                        Status = s.Status,
                                        Createddate = r.Createddate
                                    }).ToList();



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
                PatientDashboardViewModel = reqData,
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
            var userId = from t1 in _db.Requests
                         join t2 in _db.Users on t1.Userid equals t2.Userid
                         where t1.Requestid == reqId
                         select t2.Userid;
            //var requestData = from t2 in _db.Requests
            //                  join t1 in _db.Requestwisefiles
            //                  on t2.Requestid equals t1.Requestid into files
            //                  from t1 in files.DefaultIfEmpty()
            //                  where t2.Requestid == reqId
            //                  select new PatientDocumentViewModel
            //                  {
            //                      RequestId = t2.Requestid,
            //                      createdate = t2.Createddate,
            //                      Filename = t1 != null ? t1.Filename : null
            //                  };

            var requestData = from t1 in _db.Requests
                              join t3 in _db.RequestStatuses on t1.Status equals t3.StatusId
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

            var uploadData = new UploadFileViewModel
            {
                reqId = reqId,
                formFile = null
            };
            var data = new DocumentViewModel
            {
                PatientDocumentViewModel = requestData,
                UploadFileViewModel = uploadData
            };
            return View(data);
        }

        [HttpPost]
        public IActionResult Document(UploadFileViewModel obj)
        {
            var aspId = from t1 in _db.Requests
                        join t2 in _db.Users on t1.Userid equals t2.Userid
                        join t3 in _db.AspNetUsers on t2.Aspnetuserid equals t3.Id
                        where t1.Requestid == obj.reqId
                        select t3.Id;
            var id = obj.reqId;
            //uploading files
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
                    Requestid = obj.reqId,
                    Createddate = DateTime.Now
                };

                _db.Requestwisefiles.Add(requestwisefile);
                _db.SaveChanges();
            }

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
            _db.AspNetUsers.Update(existAsp);
            _db.SaveChanges();



            return RedirectToAction("Dashboard", new { AspId = aspId });
        }


    }
}
