using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.PartnersMenu;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class PartnersMenuController : Controller
    {
        private readonly IPartnersRepository _partnersRepo;
        private readonly ApplicationDbContext _db;
        private readonly INotyfService _noty;

        public PartnersMenuController(IPartnersRepository partnersRepo, ApplicationDbContext db, INotyfService noty)
        {
            _partnersRepo = partnersRepo;   
            _db = db;
            _noty = noty;
        }

        public string GetAdminName()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;

            return fname + "_" + lname;
        }

        public async Task<IActionResult> Partners()
        {
            ViewBag.AdminName = GetAdminName();

            var data = _partnersRepo.Partners();

            return View(data);
        }

        public async Task<IActionResult> VendorTable(int pageIndex, string name, int regionId)
        {
            var data = _partnersRepo.GetVendors();
            if(name != null)
            {
                data = data.Where(x => x.BusinessName.ToUpper().Contains(name.ToUpper()));
            }
            if(regionId != 0)
            {
                data = data.Where(x => x.regionId == regionId);
            }

            if(pageIndex < 1 )
            {
                pageIndex = 1;
            }
            var pageSize = 4;
            return PartialView("_Vendorstable", await PaginatedList<VendorFormViewModel>.CreateAsync(data, pageIndex, pageSize));
        }

        public IActionResult AddVendors()
        {
            ViewBag.AdminName = GetAdminName();
            var data = new VendorFormViewModel()
            {
                Healthprofessionaltypes = _db.Healthprofessionaltypes
            };
            return View(data);  
        }

        [HttpPost]
        public IActionResult AddVendors(VendorFormViewModel obj)
        {
            var vendor = new Healthprofessional()
            {
                Vendorname = obj.BusinessName,
                Faxnumber = obj.FaxNumber,
                Phonenumber = obj.Phonenumber,
                Email = obj.Email,
                Businesscontact = obj.BusinessContact,
                State = obj.State,
                City = obj.City,
                Address = obj.Street,
                Zip = obj.Zipcode,
                Profession = obj.professionTypeId
            };
            _db.Healthprofessionals.Add(vendor);
            _db.SaveChanges();

            _noty.Success("Created Vendor");
            return RedirectToAction("Partners");   
        }

        public IActionResult UpdateVendors(int id)
        {
            ViewBag.AdminName = GetAdminName();
            var vendor = _db.Healthprofessionals.Where(x => x.Vendorid == id).FirstOrDefault();
            var data = new VendorFormViewModel()
            {
                vendorId = id,
                BusinessContact = vendor.Businesscontact,
                State = vendor.State,
                City = vendor.City,
                Phonenumber = vendor.Phonenumber,
                BusinessName = vendor.Vendorname,
                FaxNumber = vendor.Faxnumber,
                Email = vendor.Email,
                Street = vendor.Address,
                Zipcode = vendor.Zip,
                professionTypeId = (int)vendor.Profession,
                Healthprofessionaltypes = _db.Healthprofessionaltypes
            };
            return View(data);
        }
        [HttpPost]
        public IActionResult UpdateVendors(VendorFormViewModel obj, int id)
        {
            ViewBag.AdminName = GetAdminName();
            var vendor = _db.Healthprofessionals.Where(x => x.Vendorid == id).FirstOrDefault();
            vendor.Vendorname = obj.BusinessName;
            vendor.Profession = obj.professionTypeId;
            vendor.Businesscontact = obj.BusinessContact;
            vendor.State = obj.State;   
            vendor.City = obj.City;
            vendor.Address = obj.Street;
            vendor.Phonenumber = obj.Phonenumber;
            vendor.Zip = obj.Zipcode;
            vendor.Faxnumber = obj.FaxNumber;
            vendor.Email = obj.Email;

            _db.Healthprofessionals.Update(vendor);
            _db.SaveChanges();

            _noty.Success("Updated Vendor");
            return RedirectToAction("Partners");
        }

        public IActionResult DeleteVendors(int id)
        {
            var vendor = _db.Healthprofessionals.Where(x => x.Vendorid == id).FirstOrDefault();
            vendor.Isdeleted = true;
            _db.Healthprofessionals.Update(vendor);
            _db.SaveChanges();


            _noty.Success("Vendor deleted");
            return RedirectToAction("Partners");
        }
    }
}
