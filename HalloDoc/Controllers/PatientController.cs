using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text;

namespace HalloDoc.Controllers
{
    [CustomAuth("patient")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPatientRepository _patientrepo;
        public PatientController(ApplicationDbContext db, IPatientRepository patientrepo)
        {
            _db = db;
            _patientrepo = patientrepo;
        }
        
        public IActionResult Dashboard(String AspId)
        {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
                var data = _patientrepo.PatientDashboard(AspId);

                return View(data);
        }

        public IActionResult Document(int reqId)
        {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
                var data = _patientrepo.Document(reqId);
                return View(data);
        }

        [HttpPost]
        public IActionResult Document(UploadFileViewModel obj)
        {
            var id = _patientrepo.Document(obj);

            return RedirectToAction("Document", "Patient", new { reqId = id });
        }


        [HttpPost]
        public IActionResult Profile(DashboardViewModel obj)
        {
            String aspId = _patientrepo.PatientProfile(obj);
            var Name = obj.ProfileEditViewModel.Firstname + " " + obj.ProfileEditViewModel.Lastname;
            HttpContext.Session.SetString("token", Name);
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
                return RedirectToAction("Dashboard", new { AspId = aspId });
        }

        public IActionResult CreateRequest(int? reqId)
        {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();

            return View();
        }

        public IActionResult CreateRequestForElse(int? reqId)
        {

            ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            return View();
        }

        [HttpPost]
        public IActionResult CreateRequest(PatientViewModel obj)
        {
            ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            if (ModelState.IsValid)
            {
                int uid = (int)HttpContext.Session.GetInt32("userId");

                var aspId = _patientrepo.CreateReqMeOrElse(obj, uid);

                return RedirectToAction("Dashboard", new { AspId = aspId });
            }
            return View();
        }


        public async Task<IActionResult> DownloadAllFiles(int requestId)
        {
            try
            {
                // Fetch all document details for the given request:
                var documentDetails = _db.Requestwisefiles.Where(m => m.Requestid == requestId).ToList();

                if (documentDetails == null || documentDetails.Count == 0)
                {
                    return NotFound("No documents found for download");
                }

                // Create a unique zip file name
                var zipFileName = $"Documents_{DateTime.Now:yyyyMMddHHmmss}.zip";
                var zipFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", zipFileName);

                // Create the directory if it doesn't exist
                var zipDirectory = Path.GetDirectoryName(zipFilePath);
                if (!Directory.Exists(zipDirectory))
                {
                    Directory.CreateDirectory(zipDirectory);
                }

                // Create a new zip archive
                using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    // Add each document to the zip archive
                    foreach (var document in documentDetails)
                    {
                        var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", document.Filename);
                        zipArchive.CreateEntryFromFile(documentPath, document.Filename);
                    }
                }

                // Return the zip file for download
                var zipFileBytes = await System.IO.File.ReadAllBytesAsync(zipFilePath);
                return File(zipFileBytes, "application/zip", zipFileName);
            }
            catch
            {
                return BadRequest("Error downloading files");
            }
        }


        public IActionResult Back()
        {
            int userId = (int)HttpContext.Session.GetInt32("userId");
            var aspId = _db.Users.FirstOrDefault(x => x.Userid == userId).Aspnetuserid;

            return RedirectToAction("Dashboard", new { AspId = aspId });
        }
    }
}
