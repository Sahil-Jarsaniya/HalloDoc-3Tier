using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HalloDoc.DataAccess.Models;

namespace HalloDoc.Controllers;

public class RequestController : Controller
{
    public IActionResult submitRequestScreen()
    {
        return View();
    }
    public IActionResult createPatientRequest()
    {
        return View();
    }
    public IActionResult createFamilyfriendRequest()
    {
        return View();
    }
    public IActionResult createConciergeRequest()
    {
        return View();
    }
    public IActionResult createBusinessRequest()
    {
        return View();
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
