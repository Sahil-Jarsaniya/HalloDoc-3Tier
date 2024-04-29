    
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
using Microsoft.AspNetCore.Routing.Matching;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace HalloDoc.Controllers;

public class RequestController : Controller
{
    private readonly IRequestRepository _requestRepo;
    private readonly INotyfService _notyf;
    public RequestController( IRequestRepository requestRepo, INotyfService notyf)
    {
        _requestRepo = requestRepo;
        _notyf = notyf;
    }

    [HttpPost]
    public JsonResult PatientCheckEmail(string email)
    {
        return Json(new { exists = _requestRepo.emailExist(email) });
    }

    public IActionResult submitRequestScreen()
    {
        return View();
    }
    public IActionResult createPatientRequest()
    {
        var data = new PatientViewModel()
        {
            Regions = _requestRepo.Regions()
        };
        return View(data);
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
            _notyf.Success("Request Submitted");
            return RedirectToAction("submitRequestScreen");
        }
        else
        {
        return View();
        }
    }

    public IActionResult createFamilyfriendRequest()
    {
        var data = new FamilyViewModel()
        {
            Regions = _requestRepo.Regions()
        };
        return View(data);
    }
    [HttpPost]
    public IActionResult createFamilyfriendRequest(FamilyViewModel obj)
    {
        if (ModelState.IsValid)
        {
            _requestRepo.CreateFamilyfriendRequest(obj);
            _notyf.Success("Request Submitted");
            return RedirectToAction("submitRequestScreen");
        }
        else
        {
            return View();
        }
    }
    public IActionResult createConciergeRequest()
    {
        var data = new ConciergeViewModel()
        {
            Regions = _requestRepo.Regions()
        };
        return View(data);
    }
    [HttpPost]
    public IActionResult createConciergeRequest(ConciergeViewModel obj)
    {
        if (ModelState.IsValid)
        {
            _requestRepo.CreateConciergeRequest(obj);
            _notyf.Success("Request Submitted");
            return RedirectToAction("submitRequestScreen");
        }
        else
        {
            return View();
        }
    }
    public IActionResult createBusinessRequest()
    {
        var data = new BussinessViewModel()
        {
            Regions = _requestRepo.Regions()
        };
        return View(data);
    }
    [HttpPost]
    public IActionResult createBusinessRequest(BussinessViewModel obj)
    {
        if(ModelState.IsValid)
        {
            _requestRepo.CreateBusinessRequest(obj);
            _notyf.Success("Request Submitted");
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