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
    private readonly IRequestRepository _requestRepo;
    public RequestController(ApplicationDbContext db, IRequestRepository requestRepo)
    {
        _db = db;
        _requestRepo = requestRepo;
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
            if(obj.Password != obj.confirmPassword)
            {
                return View();
            }
            _requestRepo.CreatePatientRequest(obj);
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
            _requestRepo.CreateFamilyfriendRequest(obj);
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
            _requestRepo.CreateConciergeRequest(obj);
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
            _requestRepo.CreateBusinessRequest(obj);
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