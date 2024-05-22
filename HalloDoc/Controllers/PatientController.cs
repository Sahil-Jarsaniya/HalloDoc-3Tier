using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Implementation;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Security.Claims;
using System.Text;

namespace HalloDoc.Controllers
{
    [CustomAuth("patient")]
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientrepo;
        private readonly INotyfService _notyf;
        private readonly IRequestRepository _requestRepo;
        private readonly IJwtService _jwtService;
        public PatientController(IPatientRepository patientrepo, INotyfService notyf, IRequestRepository requestRepo, IJwtService jwtService)
        {
            _patientrepo = patientrepo;
            _notyf = notyf;
            _requestRepo = requestRepo;
            _jwtService = jwtService;
        }

        public IActionResult Dashboard()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.Data = fname + " " + lname;
            var data = _patientrepo.PatientDashboard(AspId);

            return View(data);
        }

        public IActionResult Document(int reqId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            ViewBag.Data = fname + " " + lname;
            var data = _patientrepo.Document(reqId);
            return View(data);
        }

        [HttpPost]
        public IActionResult Document(UploadFileViewModel obj)
        {
            if (obj.formFile != null)
            {

                var id = _patientrepo.Document(obj);

                _notyf.Success("Successfully Uploaded");
                return RedirectToAction("Document", "Patient", new { reqId = id });
            }
            else
            {
                _notyf.Error("File is note Selected!!");
                return RedirectToAction("Document", "Patient", new { reqId = obj.reqId });
            }
        }

        public IActionResult MyProfile()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
            ViewBag.Data = fname + " " + lname;

            var data = _patientrepo.PatientDashboard(AspId).ProfileEditViewModel;


           


            return View(data);
        }

        [HttpPost]
        public IActionResult MyProfile(ProfileEditViewModel obj)
        {
            if (ModelState.IsValid)
            {
                String aspId = _patientrepo.PatientProfile(obj);
                var token = Request.Cookies["jwt"];
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
                string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
                string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
                string Email = jwt.Claims.First(c => c.Type == ClaimTypes.Email).Value;
                Response.Cookies.Delete("jwt");
                var user2 = new LoggedUser
                {
                    AspId = AspId,
                    FirstName = obj.Firstname,
                    LastName = obj.Lastname,
                    Email = Email,
                    Role = "patient",
                    Roleid = "0",
                };
                var jwtToken = _jwtService.GenerateJwtToken(user2);
                Response.Cookies.Append("jwt", jwtToken);
                ViewBag.Data = obj.Firstname+ " " + obj.Lastname;

                return RedirectToAction("MyProfile");
            }
            else
            {
                return View(obj);
            }
        }

        public IActionResult CreateRequest(int? reqId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            ViewBag.Data = fname + " " + lname;
            var data = new PatientViewModel()
            {
                Regions = _requestRepo.Regions()
            };
            return View(data);
        }

        public IActionResult CreateRequestForElse(int? reqId)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            ViewBag.Data = fname + " " + lname;
            var data = new FamilyViewModel()
            {
                Regions = _requestRepo.Regions()
            };
            return View(data);
        }

        [HttpPost]
        public IActionResult CreateRequest(PatientViewModel obj)
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;
            ViewBag.Data = fname + " " + lname;
            if (ModelState.IsValid)
            {
                string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;
                var aspId = _patientrepo.CreateReqMeOrElse(obj, AspId);

                return RedirectToAction("Dashboard", new { AspId = aspId });
            }
            return View();
        }

        public IActionResult ChatWithPhysician(int requestid)
        {
            var data = _patientrepo.ChatWithPhysician(requestid);
            return PartialView("_ChatView", data);
        }
        public IActionResult ChatWithAdmin(int requestid)
        {
            var data = _patientrepo.ChatWithAdmin(requestid);
            return PartialView("_ChatView", data);
        }

        public void StoreChat(int reqClientId, int senderId, string message, int AccountTypeOfReceiver)
        {
            _patientrepo.StoreChat(reqClientId, senderId, message, AccountTypeOfReceiver);
        }

        //public async Task<IActionResult> DownloadAllFiles(int requestId)
        //{
        //    try
        //    {
        //        // Fetch all document details for the given request:
        //        var documentDetails = _db.Requestwisefiles.Where(m => m.Requestid == requestId).ToList();

        //        if (documentDetails == null || documentDetails.Count == 0)
        //        {
        //            return NotFound("No documents found for download");
        //        }

        //        // Create a unique zip file name
        //        var zipFileName = $"Documents_{DateTime.Now:yyyyMMddHHmmss}.zip";
        //        var zipFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", zipFileName);

        //        // Create the directory if it doesn't exist
        //        var zipDirectory = Path.GetDirectoryName(zipFilePath);
        //        if (!Directory.Exists(zipDirectory))
        //        {
        //            Directory.CreateDirectory(zipDirectory);
        //        }

        //        // Create a new zip archive
        //        using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
        //        {
        //            // Add each document to the zip archive
        //            foreach (var document in documentDetails)
        //            {
        //                var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", document.Filename);
        //                zipArchive.CreateEntryFromFile(documentPath, document.Filename);
        //            }
        //        }

        //        // Return the zip file for download
        //        var zipFileBytes = await System.IO.File.ReadAllBytesAsync(zipFilePath);
        //        return File(zipFileBytes, "application/zip", zipFileName);
        //    }
        //    catch
        //    {
        //        return BadRequest("Error downloading files");
        //    }
        //}

        public IActionResult Back()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string AspId = jwt.Claims.First(c => c.Type == "AspId").Value;

            return RedirectToAction("Dashboard", new { AspId = AspId });
        }
    }
}
