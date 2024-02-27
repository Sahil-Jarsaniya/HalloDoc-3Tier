using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _db;

        public PatientRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public DashboardViewModel PatientDashboard(String AspId)
        {
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

            return data;
        }

        public DocumentViewModel Document(int reqId)
        {
            var userId = from t1 in _db.Requests
                         join t2 in _db.Users on t1.Userid equals t2.Userid
                         where t1.Requestid == reqId
                         select t2.Userid;

            var requestData = from t1 in _db.Requests
                              join t3 in _db.RequestStatuses on t1.Status equals t3.StatusId
                              join t2 in _db.Requestwisefiles
                              on t1.Requestid equals t2.Requestid into files
                              from t2 in files.DefaultIfEmpty()
                              where t1.Requestid == reqId
                              select new PatientDocumentViewModel
                              {
                                  RequestId = t1.Requestid,
                                  Name = t1.Firstname + " " + t1.Lastname,
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

            return data;
        }

        public int Document(UploadFileViewModel obj)
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "uploadedFiles", fileName);

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

            return id;
        }
    }
}
