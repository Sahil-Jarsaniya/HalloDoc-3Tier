using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Controllers;

public class RequestController : Controller
{
    private readonly ApplicationDbContext _db;
    public RequestController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public JsonResult PatientCheckEmail(string email)
    {
        bool emailExists = _db.Users.Any(u => u.Email == email);
        return Json(new { exists = emailExists });
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

            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();

            //Inserting into AspNetUser
            AspNetUser aspNetUser = new AspNetUser();
          
            if (existUser ==null)
            {

                aspNetUser.Id = guid.ToString();
                aspNetUser.UserName = obj.Firstname;
                aspNetUser.Email = obj.Email;
                aspNetUser.PhoneNumber = obj.Phonenumber;
                aspNetUser.CreatedDate = DateTime.UtcNow;
            _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();
            }
            //Inserting into User table
            User user = new User
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Mobile = obj.Phonenumber,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Createddate = DateTime.Now,
                Createdby = "admin"
            };
            if(existUser == null)
            {
                user.Aspnetuserid = aspNetUser.Id;
            }
            else
            {
                user.Aspnetuserid = existUser.Id;
            }

            _db.Users.Add(user);
            _db.SaveChanges();

            //Inserting into Request
            Request request = new Request
            {
                Requesttypeid = 1,
                Userid = user.Userid,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Status = 1,
                Createddate = DateTime.Now,
                Isurgentemailsent = false
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
                Status = 4,
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

            //Inserting into AspNetUser
            AspNetUser aspNetUser = new AspNetUser();

            if (existUser == null)
            {

                aspNetUser.Id = guid.ToString();
                aspNetUser.UserName = obj.Firstname;
                aspNetUser.Email = obj.Email;
                aspNetUser.PhoneNumber = obj.Phonenumber;
                aspNetUser.CreatedDate = DateTime.UtcNow;
                _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();
            }
            //Inserting into User table
            User user = new User
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Mobile = obj.Phonenumber,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Createddate = DateTime.Now,
                Createdby = "admin"
            };
            if (existUser == null)
            {
                user.Aspnetuserid = aspNetUser.Id;
            }
            else
            {
                user.Aspnetuserid = existUser.Id;
            }

            _db.Users.Add(user);
            _db.SaveChanges();

            //Inserting into request table
            Request request = new Request
            {
                Requesttypeid = 1,
                Userid = user.Userid,
                Firstname = obj.FamilyFirstname,
                Lastname = obj.FamilyLastname,
                Email = obj.FamilyEmail,
                Status = 3,
                Createddate = DateTime.Now,
                Isurgentemailsent = false
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

            //Inserting into requestStatusLog

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 4,
                Createddate = DateTime.Now
            };


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

            //Inserting into AspNetUser
            AspNetUser aspNetUser = new AspNetUser();

            if (existUser == null)
            {

                aspNetUser.Id = guid.ToString();
                aspNetUser.UserName = obj.Firstname;
                aspNetUser.Email = obj.Email;
                aspNetUser.PhoneNumber = obj.Phonenumber;
                aspNetUser.CreatedDate = DateTime.UtcNow;
                _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();
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

            //Inserting into User table
            User user = new User
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Mobile = obj.Phonenumber,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Createddate = DateTime.Now,
                Createdby = "admin"
            };
            if (existUser == null)
            {
                user.Aspnetuserid = aspNetUser.Id;
            }
            else
            {
                user.Aspnetuserid = existUser.Id;
            }

            _db.Users.Add(user);
            _db.SaveChanges();


            //Inserting into request table
            Request request = new Request
            {
                Requesttypeid = 1,
                Userid = user.Userid,
                Firstname = obj.ConciergeFirstname,
                Lastname = obj.ConciergeLastname,
                Email = obj.ConciergeEmail,
                Status = 3,
                Createddate = DateTime.Now,
                Isurgentemailsent = false
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

            //Inserting into requestStatusLog

            Requeststatuslog requeststatuslog = new Requeststatuslog
            {
                Requestid = request.Requestid,
                Status = 4,
                Createddate = DateTime.Now
            };

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
                Createdby = DateTime.Now.ToString()
            };

            var existUser = _db.AspNetUsers.FirstOrDefault(u => u.Email == obj.Email);
            Guid guid = Guid.NewGuid();

            //Inserting into AspNetUser
            AspNetUser aspNetUser = new AspNetUser();

            if (existUser == null)
            {

                aspNetUser.Id = guid.ToString();
                aspNetUser.UserName = obj.Firstname;
                aspNetUser.Email = obj.Email;
                aspNetUser.PhoneNumber = obj.Phonenumber;
                aspNetUser.CreatedDate = DateTime.UtcNow;
                _db.AspNetUsers.Add(aspNetUser);
                _db.SaveChanges();
            }
            //Inserting into User table
            User user = new User
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Mobile = obj.Phonenumber,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Createddate = DateTime.Now,
                Createdby = "admin"
            };
            if (existUser == null)
            {
                user.Aspnetuserid = aspNetUser.Id;
            }
            else
            {
                user.Aspnetuserid = existUser.Id;
            }

            _db.Users.Add(user);
            _db.SaveChanges();

            Request request = new Request
            {
                Requesttypeid = 1,
                Userid = user.Userid,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Status = 4,
                Createddate = DateTime.Now,
                Isurgentemailsent = false
            };
            _db.Requests.Add(request);
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
                Status = 4,
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