using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class RequestRepository : IRequestRepository 
    {
        private readonly ApplicationDbContext _db;
        private readonly ILoginRepository _loginRepo;
        public RequestRepository(ApplicationDbContext db, ILoginRepository loginRepository)
        {
            _db = db;
            _loginRepo = loginRepository;
        }

        public string GetConfirmationNumber(DateTime createtime, String lastName, string firstName)
        {
            String confirmationNumber = "AM" + createtime.ToString("yyMM") + lastName.ToUpper().Substring(0, Math.Min(2, lastName.Length)) + firstName.ToUpper().Substring(0, Math.Min(2, firstName.Length)) + "0001";

            return confirmationNumber;
        }
        public bool emailExist(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }
        public void CreatePatientRequest(PatientViewModel obj)
        {

            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;

            if (existUser == null)
            {
                if(obj.Password != null)
                {
                var hashPass = _loginRepo.GetHash(obj.Password);
                AspNetUser aspNetUser = new AspNetUser
                {

                    Id = guid.ToString(),
                    PasswordHash = hashPass,
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = obj.countryCode + obj.Phonenumber,
                    Email = obj.Email,
                };
                _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();
                }
                else
                {
                    AspNetUser aspNetUser = new AspNetUser
                    {

                        Id = guid.ToString(),
                        UserName = obj.Email,
                        CreatedDate = DateTime.UtcNow,
                        PhoneNumber = obj.countryCode + obj.Phonenumber,
                        Email = obj.Email,
                    };
                    _db.AspNetUsers.Add(aspNetUser);
                    _db.SaveChanges();
                    string subject = "Registration Link";
                    string body = "link";
                    _loginRepo.SendEmail(obj.Email, subject, body);
                }


                User user = new User
                {
                    Aspnetuserid = guid.ToString(),
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Mobile = obj.countryCode + obj.Phonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Createddate = DateTime.Now,
                    Strmonth = obj.Strmonth,
                    Createdby = "admin"
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                uid = user.Userid;
            }
            else
            {
                var user = _db.Users.FirstOrDefault(u => u.Aspnetuserid == existUser.Id);
                uid = user.Userid;
            }

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
                Phonenumber = obj.countryCode + obj.Phonenumber,
                Confirmationnumber = GetConfirmationNumber(DateTime.Now, obj.Lastname, obj.Firstname)
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
                Phonenumber = obj.countryCode[2] + obj.Phonenumber,
                Strmonth = obj.Strmonth,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Notes = obj.Notes
            };
            _db.Requestclients.Add(requestclient);
            _db.SaveChanges();
            //Inserting into requestStatusLog

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 1,
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requeststatuslog);
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
        }

        public void CreateFamilyfriendRequest(FamilyViewModel obj)
        {
            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;

            string[] contryCode = obj.familyCountryFlag.Split("+");

            if (existUser == null)
            {

                AspNetUser aspNetUser = new AspNetUser
                {
                    Id = guid.ToString(),
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber ="+"+contryCode[2]+ obj.Phonenumber,
                    Email = obj.Email,
                };
                _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();

                User user = new User
                {
                    Aspnetuserid = guid.ToString(),
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Mobile = "+" + contryCode[2] + obj.Phonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Createddate = DateTime.Now,
                    Strmonth = obj.Strmonth,
                    Createdby = "admin"
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                uid = user.Userid;
            }
            else
            {
                string subject = "Registration Link";
                string body = "link";
                _loginRepo.SendEmail(obj.Email, subject, body);
                var user = _db.Users.FirstOrDefault(u => u.Aspnetuserid == existUser.Id);
                uid = user.Userid;
            }

            //Inserting into request table
            Request request = new Request
            {
                Requesttypeid = 2,
                Userid = uid,
                Firstname = obj.FamilyFirstname,
                Lastname = obj.FamilyLastname,
                Email = obj.FamilyEmail,
                Status = 1,
                Createddate = DateTime.Now,
                Isurgentemailsent = false,
                Phonenumber = "+" + contryCode[1] + obj.FamilyPhonenumber,
                Confirmationnumber = GetConfirmationNumber(DateTime.Now, obj.Lastname, obj.Firstname)
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
                Phonenumber = "+" + contryCode[2] + obj.Phonenumber,
                Strmonth = obj.Strmonth,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Notes = obj.Notes
            };
            _db.Requestclients.Add(requestclient);
            _db.SaveChanges();

            //Inserting into requestStatusLog

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 1,
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requeststatuslog);
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
        }

        public void CreateConciergeRequest(ConciergeViewModel obj)
        {
            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;

            string[] contryCode = obj.ConciergeCountryCode.Split("+");

            if (existUser == null)
            {

                AspNetUser aspNetUser = new AspNetUser
                {
                    Id = guid.ToString(),
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = "+" + contryCode[2] + obj.Phonenumber,
                    Email = obj.Email,
                };
                _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();

                User user = new User
                {
                    Aspnetuserid = guid.ToString(),
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Mobile = "+" + contryCode[2] + obj.Phonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Createddate = DateTime.Now,
                    Strmonth = obj.Strmonth,
                    Createdby = "admin"
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                uid = user.Userid;
                string subject = "Registration Link";
                string body = "link";
                _loginRepo.SendEmail(obj.Email, subject, body);
            }
            else
            {
                var user = _db.Users.FirstOrDefault(u => u.Aspnetuserid == existUser.Id);
                uid = user.Userid;
            }
            //inserting into concierge table
            Concierge concierge = new Concierge
            {
                Conciergename = obj.ConciergeFirstname + " " + obj.ConciergeLastname,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Createddate = DateTime.Now,
                Regionid = 1 // region table refernce
            };

            _db.Concierges.Add(concierge);
            _db.SaveChanges();

            //Inserting into request table
            Request request = new Request
            {
                Requesttypeid = 3,
                Userid = uid,
                Firstname = obj.ConciergeFirstname,
                Lastname = obj.ConciergeLastname,
                Email = obj.ConciergeEmail,
                Status = 1,
                Createddate = DateTime.Now,
                Isurgentemailsent = false,
                Phonenumber ="+"+ contryCode[1] + obj.ConciergePhonenumber,
                Confirmationnumber = GetConfirmationNumber(DateTime.Now, obj.Lastname, obj.Firstname)
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
                Phonenumber = "+" + contryCode[2] + obj.Phonenumber,
                Strmonth = obj.Strmonth,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Notes = obj.Notes
            };
            _db.Requestclients.Add(requestclient);
            _db.SaveChanges();
            //Inserting into requestStatusLog

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 1,
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requeststatuslog);
            _db.SaveChanges();
        }

        public void CreateBusinessRequest(BussinessViewModel obj)
        {
            Business business = new Business
            {
                Name = obj.bussinessFirstname + " " + obj.bussinessLastname,
                Createddate = DateTime.Now
            };
            _db.Businesses.Add(business);
            _db.SaveChanges();

            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;

            string[] contryCode = obj.businessCountryFlag.Split("+");

            if (existUser == null)
            {
                AspNetUser aspNetUser = new AspNetUser
                {
                    Id = guid.ToString(),
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = "+"+ contryCode[2] +obj.Phonenumber,
                    Email = obj.Email,
                };
                _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();

                User user = new User
                {
                    Aspnetuserid = guid.ToString(),
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Mobile = "+" + contryCode[2]+ obj.Phonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Createddate = DateTime.Now,
                    Strmonth = obj.Strmonth,
                    Createdby = "admin"
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                uid = user.Userid;

                string subject = "Registration Link";
                string body = "link";
                _loginRepo.SendEmail(obj.Email, subject, body);
            }
            else
            {
                var user = _db.Users.FirstOrDefault(u => u.Aspnetuserid == existUser.Id);
                uid = user.Userid;
            }

            Request request = new Request
            {
                Requesttypeid = 4,
                Userid = uid,
                Firstname = obj.bussinessFirstname,
                Lastname = obj.bussinessLastname,
                Email = obj.bussinessEmail,
                Status = 1,
                Createddate = DateTime.Now,
                Isurgentemailsent = false,
                Phonenumber = "+" + contryCode[1]+obj.bussinessPhonenumber,
                Confirmationnumber = GetConfirmationNumber(DateTime.Now, obj.Lastname, obj.Firstname)
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
                Phonenumber = "+" + contryCode[2]+ obj.Phonenumber,
                Strmonth = obj.Strmonth,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Notes = obj.Notes
            };
            _db.Requestclients.Add(requestclient);
            _db.SaveChanges();

            Requestbusiness requestbusiness = new Requestbusiness
            {
                Businessid = business.Id,
                Requestid = request.Requestid
            };
            _db.Requestbusinesses.Add(requestbusiness);
            _db.SaveChanges();

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 1,
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requeststatuslog);
            _db.SaveChanges();
        }

        public void CreateRequestByPhysician(PatientViewModel obj, int PhyId)
        {

            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;


            if (existUser == null)
            {
                if (obj.Password != null)
                {
                    var hashPass = _loginRepo.GetHash(obj.Password);
                    AspNetUser aspNetUser = new AspNetUser
                    {

                        Id = guid.ToString(),
                        PasswordHash = hashPass,
                        UserName = obj.Email,
                        CreatedDate = DateTime.UtcNow,
                        PhoneNumber = obj.countryCode+obj.Phonenumber,
                        Email = obj.Email,
                    };
                    _db.AspNetUsers.Add(aspNetUser);
                    _db.SaveChanges();
                }
                else
                {
                    AspNetUser aspNetUser = new AspNetUser
                    {

                        Id = guid.ToString(),
                        UserName = obj.Email,
                        CreatedDate = DateTime.UtcNow,
                        PhoneNumber = obj.countryCode + obj.Phonenumber,
                        Email = obj.Email,
                    };
                    _db.AspNetUsers.Add(aspNetUser);
                    _db.SaveChanges();
                    string subject = "Registration Link";
                    string body = "link";
                    _loginRepo.SendEmail(obj.Email, subject, body);
                }


                User user = new User
                {
                    Aspnetuserid = guid.ToString(),
                    Firstname = obj.Firstname,
                    Lastname = obj.Lastname,
                    Email = obj.Email,
                    Mobile = obj.countryCode + obj.Phonenumber,
                    Street = obj.Street,
                    City = obj.City,
                    State = obj.State,
                    Zipcode = obj.Zipcode,
                    Createddate = DateTime.Now,
                    Strmonth = obj.Strmonth,
                    Createdby = "admin"
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                uid = user.Userid;
            }
            else
            {
                var user = _db.Users.FirstOrDefault(u => u.Aspnetuserid == existUser.Id);
                uid = user.Userid;
            }

            //Inserting into Request
            Request request = new Request
            {
                Physicianid = PhyId,
                Requesttypeid = 1,
                Userid = uid,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Status = 2,
                Createddate = DateTime.Now,
                Isurgentemailsent = false,
                Phonenumber = obj.countryCode + obj.Phonenumber,
                Confirmationnumber = GetConfirmationNumber(DateTime.Now, obj.Lastname, obj.Firstname)
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
                Phonenumber = obj.countryCode + obj.Phonenumber,
                Strmonth = obj.Strmonth,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Notes = obj.Notes
            };
            _db.Requestclients.Add(requestclient);
            _db.SaveChanges();
            //Inserting into requestStatusLog

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 1,
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requeststatuslog);
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
        }
    }
}
