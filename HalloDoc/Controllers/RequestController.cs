using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using HalloDoc.BussinessAccess.Repository.Interface;

namespace HalloDoc.Controllers;

public class RequestController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IPatientRepository _patientRepo;
    private readonly ILoginRepository _login;
    public RequestController(ApplicationDbContext db, IPatientRepository patientRepo, ILoginRepository login)
    {
        _db = db;
        _patientRepo = patientRepo;
        _login = login; 
    }

    [HttpPost]
    public JsonResult PatientCheckEmail(string email)
    {
        bool emailExists = _db.Users.Any(u => u.Email == email);
        return Json(new { exists = emailExists });
    }

    public static string GetHash(string text)
    {
        // SHA512 is disposable by inheritance.  
        using (var sha256 = SHA256.Create())
        {
            // Send a sample text to hash.  
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            // Get the hashed string.  
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    public static string GetConfirmationNumber( DateTime createtime, String lastName, string firstName)
    {
        String confirmationNumber = "AM" + createtime.ToString("yyMM") + lastName.ToUpper().Substring(0, Math.Min(2, lastName.Length))+ firstName.ToUpper().Substring(0, Math.Min(2, firstName.Length)) + "0001";

        return confirmationNumber;
    }

    public IActionResult submitRequestScreen()
    {
        return View();
    }
    public IActionResult createPatientRequest()
    {
        return View();
    }
    [HttpPost]
    public IActionResult createPatientRequest(PatientViewModel obj)
    {
        if (ModelState.IsValid)
        {
            if(obj.Password != obj.confirmPassword)
            {
                return View();
            }
            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;

            if (existUser == null)
            {
                var hashPass = GetHash(obj.Password);

                AspNetUser aspNetUser = new AspNetUser
                {

                    Id = guid.ToString(),
                    Password = hashPass,
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = obj.Phonenumber,
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
                    Mobile = obj.Phonenumber,
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
                Phonenumber = obj.Phonenumber,
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
            if(obj.formFile != null && obj.formFile.Length > 0)
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

            return RedirectToAction("submitRequestScreen");
        }
        else
        {
        return View();
        }
    }

    public IActionResult createFamilyfriendRequest()
    {
        return View();
    }
    [HttpPost]
    public IActionResult createFamilyfriendRequest(FamilyViewModel obj)
    {
        if (ModelState.IsValid)
        {
            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;

            

            if (existUser == null)
            {
                
                AspNetUser aspNetUser = new AspNetUser
                {
                    Id = guid.ToString(),
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = obj.Phonenumber,
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
                    Mobile = obj.Phonenumber,
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
                _login.SendEmail(obj.Email);
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
                Phonenumber = obj.FamilyPhonenumber,
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

            return RedirectToAction("submitRequestScreen");
        }
        else
        {
            return View();
        }
    }
    public IActionResult createConciergeRequest()
    {
        return View();
    }
    [HttpPost]
    public IActionResult createConciergeRequest(ConciergeViewModel obj)
    {
        if (ModelState.IsValid)
        {

            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();
            var uid = 0;
            if (existUser == null)
            {

                AspNetUser aspNetUser = new AspNetUser
                {
                    Id = guid.ToString(),
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = obj.Phonenumber,
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
                    Mobile = obj.Phonenumber,
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
                 _login.SendEmail(obj.Email);
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
                Phonenumber = obj.ConciergePhonenumber,
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
            //Inserting into requestStatusLog

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 1,
                Createddate = DateTime.Now
            };
            _db.Requeststatuslogs.Add(requeststatuslog);
            _db.SaveChanges();
            return RedirectToAction("submitRequestScreen");
        }
        else
        {
            return View();
        }
    }
    public IActionResult createBusinessRequest()
    {
        return View();
    }
    [HttpPost]
    public IActionResult createBusinessRequest(BussinessViewModel obj)
    {
        if(ModelState.IsValid)
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
           if(existUser == null)
            {
                var hashPass = GetHash(obj.Password);
                AspNetUser aspNetUser = new AspNetUser
                {
                    Id = guid.ToString(),
                    UserName = obj.Email,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = obj.Phonenumber,
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
                     Mobile = obj.Phonenumber,
                     Street = obj.Street,
                     City = obj.City,
                     State  = obj.State,
                     Zipcode = obj.Zipcode,
                     Createddate = DateTime.Now,
                     Strmonth = obj.Strmonth,
                     Createdby = "admin"
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                uid = user.Userid;

                _login.SendEmail(obj.Email);
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
                Phonenumber = obj.bussinessPhonenumber,
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

            return RedirectToAction("submitRequestScreen");
        }
        else
        {
        return View();
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}