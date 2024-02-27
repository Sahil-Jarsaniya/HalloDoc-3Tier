﻿using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text;

namespace HalloDoc.Controllers
{
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
            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
                var data = _patientrepo.PatientDashboard(AspId);

                return View(data);
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public IActionResult Document(int reqId)
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
                var data = _patientrepo.Document(reqId);
                return View(data);
            }
            else
            {
                return RedirectToAction("login");
            }
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
            var aspId = from t1 in _db.AspNetUsers
                        join t2 in _db.Users on t1.Id equals t2.Aspnetuserid
                        where t2.Userid == obj.ProfileEditViewModel.UserId
                        select t1.Id;



            var existUser = _db.Users.FirstOrDefault(x => x.Userid == obj.ProfileEditViewModel.UserId);
            var email = existUser.Email;

            existUser.Firstname = obj.ProfileEditViewModel.Firstname;
            existUser.Lastname = obj.ProfileEditViewModel.Lastname;
            existUser.Email = obj.ProfileEditViewModel.Email;
            existUser.Mobile = obj.ProfileEditViewModel.Phonenumber;
            existUser.Street = obj.ProfileEditViewModel.Street;
            existUser.City = obj.ProfileEditViewModel.City;
            existUser.State = obj.ProfileEditViewModel.State;
            existUser.Zipcode = obj.ProfileEditViewModel.Zipcode;
            existUser.Modifieddate = DateTime.Now;
            _db.Users.Update(existUser);
            _db.SaveChanges();

            var existAsp = _db.AspNetUsers.FirstOrDefault(x => x.Id == existUser.Aspnetuserid);

            existAsp.Email = obj.ProfileEditViewModel.Email;
            existAsp.PhoneNumber = obj.ProfileEditViewModel.Phonenumber;
            existAsp.ModifiedDate = DateTime.UtcNow;

            _db.AspNetUsers.Update(existAsp);
            _db.AspNetUsers.Update(existAsp);
            _db.SaveChanges();

            _db.Requests.Where(x => x.Userid == existUser.Userid).ToList().ForEach(x =>
            {
                x.Firstname = obj.ProfileEditViewModel.Firstname;
                x.Lastname = obj.ProfileEditViewModel.Lastname;
                x.Email = obj.ProfileEditViewModel.Email;
                x.Phonenumber = obj.ProfileEditViewModel.Phonenumber;
                x.Modifieddate = DateTime.Now;

                _db.Requests.Update(x);
                _db.SaveChanges();
            });

            _db.Requestclients.Where(x => x.Email == email).ToList().ForEach(x =>
            {
                x.Firstname = obj.ProfileEditViewModel.Firstname;
                x.Lastname = obj.ProfileEditViewModel.Lastname;
                x.Email = obj.ProfileEditViewModel.Email;
                x.Phonenumber = obj.ProfileEditViewModel.Phonenumber;
                x.Street = obj.ProfileEditViewModel.Street;
                x.City = obj.ProfileEditViewModel.City;
                x.State = obj.ProfileEditViewModel.State;
                x.Zipcode = obj.ProfileEditViewModel.Zipcode;
                _db.Requestclients.Update(x);
                _db.SaveChanges();
            });

            HttpContext.Session.SetString("token", existUser.Firstname + " " + existUser.Lastname);
            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            }
            else
            {
                return RedirectToAction("login");
            }
            return RedirectToAction("Dashboard", new { AspId = aspId });
        }

        public IActionResult CreateRequest(int? reqId)
        {

            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            }
            else
            {
                return RedirectToAction("login");
            }


            return View();
        }

        public IActionResult CreateRequestForElse(int? reqId)
        {

            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            }
            else
            {
                return RedirectToAction("login");
            }


            return View();
        }

        [HttpPost]
        public IActionResult CreateRequest(PatientViewModel obj)
        {

            if (HttpContext.Session.GetString("token") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("token").ToString();
            }
            else
            {
                return RedirectToAction("login");
            }

            if (ModelState.IsValid)
            {
                int uid = (int)HttpContext.Session.GetInt32("userId");

                var aspId = _db.Users.Where(x => x.Userid == uid).FirstOrDefault().Aspnetuserid;

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
