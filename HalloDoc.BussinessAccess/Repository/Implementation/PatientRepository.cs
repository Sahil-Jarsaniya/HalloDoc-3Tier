using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.utils;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;

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
                               fileCount = _db.Requestwisefiles.Where(x => x.Requestid == r.Requestid).Count(),
                               Status = s.Status,
                               Createddate = r.Createddate,
                               phyId = r.Physicianid ?? 0
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
                              join t4 in _db.Admins on t2.Adminid equals t4.Adminid into admin
                              from t4 in admin.DefaultIfEmpty()
                              join t5 in _db.Physicians on t2.Physicianid equals t5.Physicianid into physicians
                              from t5 in physicians.DefaultIfEmpty()
                              where t1.Requestid == reqId
                              select new PatientDocumentViewModel
                              {
                                  RequestId = t1.Requestid,
                                  Name = t4.Adminid != null ? (t4.Firstname + " " + t4.Lastname) : (t5.Physicianid != null ? (t5.Firstname + " " + t5.Lastname) : (t1.Firstname + " " + t1.Lastname)),
                                  createdate = t2.Createddate,
                                  Filename = t2 != null ? t2.Filename : null
                              };
            var reqClientRow = _db.Requestclients.Where(x => x.Requestid == reqId).FirstOrDefault();
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", "RequestData\\" + id);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    Console.WriteLine("Folder created successfully.");
                }

                var fileaaaa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", "RequestData\\" + id, fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(fileaaaa, FileMode.Create))
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

        public String PatientProfile(ProfileEditViewModel obj)
        {

            var existUser = _db.Users.FirstOrDefault(x => x.Userid == obj.UserId);
            var email = existUser.Email;

            existUser.Firstname = obj.Firstname;
            existUser.Lastname = obj.Lastname;
            existUser.Street = obj.Street;
            existUser.City = obj.City;
            existUser.State = obj.State;
            existUser.Zipcode = obj.Zipcode;
            existUser.Modifieddate = DateTime.Now;
            _db.Users.Update(existUser);
            _db.SaveChanges();

            _db.Requests.Where(x => x.Userid == existUser.Userid).ToList().ForEach(x =>
            {
                x.Firstname = obj.Firstname;
                x.Lastname = obj.Lastname;
                x.Modifieddate = DateTime.Now;

                _db.Requests.Update(x);
                _db.SaveChanges();
            });

            _db.Requestclients.Where(x => x.Email == email).ToList().ForEach(x =>
            {
                x.Firstname = obj.Firstname;
                x.Lastname = obj.Lastname;
                x.Street = obj.Street;
                x.City = obj.City;
                x.State = obj.State;
                x.Zipcode = obj.Zipcode;
                _db.Requestclients.Update(x);
                _db.SaveChanges();
            });

            return existUser.Aspnetuserid;
        }

        public String CreateReqMeOrElse(PatientViewModel obj, string aspId)
        {

            var uid = _db.Users.Where(x => x.Aspnetuserid == aspId).FirstOrDefault().Userid;

            //Inserting into Request
            Request request = new Request
            {
                Requesttypeid = (int)enumsFile.RequestType.patient,
                Userid = uid,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Status = (int)enumsFile.requestStatus.Unassigned,
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

        public AgreementViewModel ReviewAgreement(String reqClientId)
        {
            int id = int.Parse(reqClientId);
            var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == id).FirstOrDefault();

            var data = new AgreementViewModel
            {
                Requestclientid = id,
                Firstname = reqClientRow.Firstname,
                Lastname = reqClientRow.Lastname,
            };

            return data;
        }
        public void Agree(int reqClientId)
        {
            var reqId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqId.Requestid).FirstOrDefault();

            reqRow.Status = (int)enumsFile.requestStatus.Consult;
            _db.Requests.Update(reqRow);
            _db.SaveChanges();
        }
        public void DisAgree(AgreementViewModel obj)
        {
            var reqId = _db.Requestclients.Where(x => x.Requestclientid == obj.Requestclientid).FirstOrDefault();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqId.Requestid).FirstOrDefault();

            reqRow.Status = (int)enumsFile.requestStatus.CancelledByPatient;
            reqRow.Casetag = obj.CancelNote;
            _db.Requests.Update(reqRow);
            _db.SaveChanges();

            var reqStatusLog = new Requeststatuslog
            {
                Requestid = reqRow.Requestid,
                Notes = obj.CancelNote,
                Createddate = DateTime.Now,
                Status = (int)enumsFile.requestStatus.CancelledByPatient
            };
            _db.Requeststatuslogs.Add(reqStatusLog);
            _db.SaveChanges();
        }


        public Chat ChatWithPhysician(int requestid)
        {
            var reqClientRow = _db.Requestclients.FirstOrDefault(x => x.Requestid == requestid);
            var reqRow = _db.Requests.FirstOrDefault(x => x.Requestid == requestid);
            var phyRow = _db.Physicians.FirstOrDefault(x => x.Physicianid == reqRow.Physicianid);
            var history = from t1 in _db.PatientChats
                          join t2 in _db.Requestclients on t1.ReqClientId equals t2.Requestclientid
                          where t1.PatientUserId == reqRow.Userid && t1.ReqClientId == reqClientRow.Requestclientid
                          select new ChatHistory()
                          {
                              Message = t1.Message,
                              CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                              CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                              Sender = t2.Firstname + " " + t2.Lastname,
                              isMyMsg = true,
                          };
            var phyHistory = (from t1 in _db.PhysicianChats
                              join t2 in _db.Physicians on t1.PhysicianId equals t2.Physicianid
                              where t1.ReqClientId == reqClientRow.Requestclientid && t1.PhysicianId == phyRow.Physicianid
                              select new ChatHistory()
                              {
                                  Message = t1.Message,
                                  CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                                  CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                                  Sender = t2.Firstname + " " + t2.Lastname,
                                  isMyMsg = false
                              });
            var list = history.Union(phyHistory);
            list = list.OrderBy(x => x.CreatedOn).ThenBy(X => X.CreatedAt);

            var data = new Chat()
            {
                Sender = reqClientRow.Firstname + " " + reqClientRow.Lastname,
                SenderId = reqRow.Userid ?? 0,
                reqClientId = reqClientRow.Requestclientid,
                chatHistories = list?.ToList() ,
                Receiver = phyRow.Firstname + " " + phyRow.Lastname,
                AccountTypeOfSender = (int)enumsFile.AccountType.Patient,
                AccountTypeOfReceiver = (int)enumsFile.AccountType.Physician
            };
            return data;

        }
        public Chat ChatWithAdmin(int requestid)
        {
            var reqClientRow = _db.Requestclients.FirstOrDefault(x => x.Requestid == requestid);
            var reqRow = _db.Requests.FirstOrDefault(x => x.Requestid == requestid);
            var history = from t1 in _db.PatientChats
                          join t2 in _db.Requestclients on t1.ReqClientId equals t2.Requestclientid
                          where t1.PatientUserId == reqRow.Userid && t1.ReqClientId == reqClientRow.Requestclientid
                          select new ChatHistory()
                          {
                              Message = t1.Message,
                              isMyMsg = true,
                              Sender = t2.Firstname + " " + t2.Lastname,
                              CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                              CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                          };
            var adminHistory = (from t1 in _db.AdminChats
                                join t2 in _db.Admins on t1.AdminId equals t2.Adminid
                                where t1.ReqClientId == reqClientRow.Requestclientid
                                select new ChatHistory()
                                {
                                    Message = t1.Message,
                                    isMyMsg = false,
                                    Sender = t2.Firstname + " " + t2.Lastname,
                                    CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                                    CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                                });
            var list = history.Union(adminHistory);
            list = list.OrderBy(x => x.CreatedOn).ThenBy(X => X.CreatedAt);

            var data = new Chat()
            {
                Sender = reqClientRow.Firstname + " " + reqClientRow.Lastname,
                SenderId = reqRow.Userid ?? 0,
                reqClientId = reqClientRow.Requestclientid,
                chatHistories = list?.ToList(),
                AccountTypeOfSender = (int)enumsFile.AccountType.Patient,
                AccountTypeOfReceiver = (int)enumsFile.AccountType.Admin
            };
            return data;
        }

        public void StoreChat(int reqClientId, int PatientUserId, string message)
        {
            var chat = new PatientChat()
            {
                PatientUserId = PatientUserId,
                ReqClientId = reqClientId,
                CreateTime = DateTime.UtcNow,
                Message = message,
                SenderAccountType = 3,
            };
            _db.PatientChats.Add(chat);
            _db.SaveChanges();
        }
    }
}
