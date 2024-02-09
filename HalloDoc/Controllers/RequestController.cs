using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.Data;

namespace HalloDoc.Controllers;

public class RequestController : Controller
{
    private readonly ApplicationDbContext _db;
    public RequestController(ApplicationDbContext db)
    {
        _db = db;
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
            Guid guid = Guid.NewGuid();

            //Inserting into AspNetUser

            //    AspNetUser aspNetUser = new AspNetUser();
            //if (!aspNetUser.Email.Contains(obj.Email.ToString()))
            //{
            //    aspNetUser.Id = guid.ToString();
            //    aspNetUser.UserName = obj.Firstname;
            //    aspNetUser.Email = obj.Email;
            //    aspNetUser.PhoneNumber = obj.Phonenumber;
            //    aspNetUser.CreatedDate = DateTime.Now;
            //    _db.AspNetUsers.Add(aspNetUser);
            //    _db.SaveChanges();

            //}

            //Inserting into User table
            User user = new User();    
            user.Firstname = obj.Firstname;
            user.Lastname = obj.Lastname;
            user.Email = obj.Email;
            user.Mobile = obj.Phonenumber;
            user.Street = obj.Street;
            user.City = obj.City;
            user.State = obj.State;
            user.Zipcode = obj.Zipcode;
            user.Createddate = DateTime.Now;
            user.Createdby = "admin";
            _db.Users.Add(user);
            _db.SaveChanges();

            //Inserting into Request
            Request request = new Request();
            request.Requesttypeid = 1;
            request.Userid = user.Userid;
            request.Firstname = obj.Firstname;
            request.Lastname = obj.Lastname;
            request.Email = obj.Email;
            request.Status = 1;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = false;
            _db.Requests.Add(request);
            _db.SaveChanges();
            //Insertung into RequestClient
            Requestclient requestclient = new Requestclient();

            requestclient.Requestid = request.Requestid;
            requestclient.Firstname = obj.Firstname;
            requestclient.Lastname = obj.Lastname;
            requestclient.Email = obj.Email;
            requestclient.Phonenumber = obj.Phonenumber;
            requestclient.Strmonth = obj.Strmonth;
            requestclient.Street = obj.Street;
            requestclient.City = obj.City;
            requestclient.State = obj.State;
            requestclient.Zipcode = obj.Zipcode;
            requestclient.Notes = obj.Notes;

            _db.Requestclients.Add(requestclient);
            _db.SaveChanges();
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
    public IActionResult createConciergeRequest()
    {
        return View();
    }
    [HttpPost]
    public IActionResult createConciergeRequest(ConciergeViewModel obj)
    {
        if (ModelState.IsValid)
        {
        //inserting into concierge table
        Concierge concierge = new Concierge();
            concierge.Conciergename = obj.ConciergeFirstname+ " " + obj.ConciergeLastname;
            concierge.Street = obj.Street;
            concierge.City = obj.City;
            concierge.State = obj.State;
            concierge.Zipcode = obj.Zipcode;
            concierge.Createddate = DateTime.Now;
            concierge.Regionid = 1; // region table refernce

            _db.Concierges.Add(concierge);
            _db.SaveChanges();

        //inserting into user table
        User user = new User();
        user.Firstname = obj.Firstname;
        user.Lastname = obj.Lastname;
        user.Email = obj.Email;
        user.Mobile = obj.Phonenumber;
        user.Street = obj.Street;
        user.City = obj.City;
        user.State = obj.State;
        user.Zipcode = obj.Zipcode;
        user.Createddate = DateTime.Now;
        user.Createdby = "admin";
        _db.Users.Add(user);
        _db.SaveChanges();

        //Inserting into request table
        Request request = new Request();
        request.Requesttypeid = 1;
        request.Userid = user.Userid;
        request.Firstname = obj.ConciergeFirstname;
        request.Lastname = obj.ConciergeLastname;
        request.Email = obj.ConciergeEmail;
        request.Status = 3;
        request.Createddate = DateTime.Now;
        request.Isurgentemailsent = false;
        _db.Requests.Add(request);
        _db.SaveChanges();

        //Insertung into RequestClient
        Requestclient requestclient = new Requestclient();

         requestclient.Requestid = request.Requestid;
         requestclient.Firstname = obj.Firstname;
         requestclient.Lastname = obj.Lastname;
         requestclient.Email = obj.Email;
         requestclient.Phonenumber = obj.Phonenumber;
         requestclient.Strmonth = obj.Strmonth;
         requestclient.Street = obj.Street;
         requestclient.City = obj.City;
         requestclient.State = obj.State;
         requestclient.Zipcode = obj.Zipcode;
         requestclient.Notes = obj.Notes;

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
            Business business = new Business();
            


            User user = new User();

            user.Userid = 2;
            user.Firstname = obj.Firstname;
            user.Lastname = obj.Lastname;
            user.Email = obj.Email;
            user.Mobile = obj.Phonenumber;
            user.Street = obj.Street;
            user.City = obj.City;
            user.State = obj.State;
            user.Zipcode = obj.Zipcode;
            user.Createddate = DateTime.Now;
            user.Createdby = "admin";
            _db.Users.Add(user);
            _db.SaveChanges();

            Request request = new Request();
            request.Requesttypeid = 1;
            request.Userid = user.Userid;
            request.Firstname = obj.Firstname;
            request.Lastname = obj.Lastname;
            request.Email = obj.Email;
            request.Status = 4;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = false;
            _db.Requests.Add(request);
            _db.SaveChanges();

            Requestbusiness requestbusiness = new Requestbusiness();

            requestbusiness.Businessid = business.Id;
            requestbusiness.Requestid = request.Requestid;


            return RedirectToAction("submitScreenRequest");
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