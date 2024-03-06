using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;
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
            var reqClientRow = _db.Requestclients.Where(x =>x.Requestid == reqId).FirstOrDefault();
            var uploadData = new UploadFileViewModel
            {
                reqId = reqId,
                formFile = null
            };
            var data = new DocumentViewModel
            {
                PatientName = reqClientRow.Firstname + " " + reqClientRow.Lastname,
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

            return id;
        }

        public String PatientProfile(DashboardViewModel obj)
        {
          
            var existUser = _db.Users.FirstOrDefault(x => x.Userid == obj.ProfileEditViewModel.UserId);
            var email = existUser.Email;

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

            _db.Requests.Where(x => x.Userid == existUser.Userid).ToList().ForEach(x =>
            {
                x.Firstname = obj.ProfileEditViewModel.Firstname;
                x.Lastname = obj.ProfileEditViewModel.Lastname;
                x.Email = obj.ProfileEditViewModel.Email;
                x.Phonenumber = obj.ProfileEditViewModel.Phonenumber;
                x.Modifieddate = DateTime.Now;

                _db.Requests.Update(x);
                _db.SaveChanges();
            });

            _db.Requestclients.Where(x => x.Email == email).ToList().ForEach(x =>
            {
                x.Firstname = obj.ProfileEditViewModel.Firstname;
                x.Lastname = obj.ProfileEditViewModel.Lastname;
                x.Email = obj.ProfileEditViewModel.Email;
                x.Phonenumber = obj.ProfileEditViewModel.Phonenumber;
                x.Street = obj.ProfileEditViewModel.Street;
                x.City = obj.ProfileEditViewModel.City;
                x.State = obj.ProfileEditViewModel.State;
                x.Zipcode = obj.ProfileEditViewModel.Zipcode;
                _db.Requestclients.Update(x);
                _db.SaveChanges();
            });

            return existAsp.Id;
        }

        public String CreateReqMeOrElse(PatientViewModel obj, int uid)
        {
            
                

                var aspId = _db.Users.Where(x => x.Userid == uid).FirstOrDefault().Aspnetuserid;

                //Inserting into Request
                Request request = new Request
                {
                    Requesttypeid = 1,
                    Userid = uid,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Status = 1,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = false,
                    Relationname = obj.Relationname
                };
                _db.Requests.Add(request);
                _db.SaveChanges();
                //Insertung into RequestClient
                Requestclient requestclient = new Requestclient
                {
                    Requestid = request.Requestid,
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Phonenumber = obj.Phonenumber,
                    Strmonth = obj.Strmonth,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Notes = obj.Notes
                };
                _db.Requestclients.Add(requestclient);
                _db.SaveChanges();

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
                        Requestid = request.Requestid,
                        Createddate = DateTime.Now
                    };

                    _db.Requestwisefiles.Add(requestwisefile);
                    _db.SaveChanges();
                }

                return aspId;
            }
    }
}
