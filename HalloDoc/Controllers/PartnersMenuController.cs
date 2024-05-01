    using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.PartnersMenu;
using HalloDoc.Services;
using HalloDoc.DataAccess.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class PartnersMenuController : Controller
    {
        private readonly IPartnersRepository _partnersRepo;
        private readonly ILoginRepository _loginRepo;
        private readonly INotyfService _noty;

        public PartnersMenuController(IPartnersRepository partnersRepo, INotyfService noty, ILoginRepository loginRepo)
        {
            _partnersRepo = partnersRepo;
            _noty = noty;
            _loginRepo = loginRepo;
        }

        public string GetAdminName()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;

            return fname + "_" + lname;
        }


        [RoleAuth((int)enumsFile.adminRoles.Vendersinfo)]
        public async Task<IActionResult> Partners()
        {
            ViewBag.AdminName = GetAdminName();

            var data = _partnersRepo.Partners();

            return View(data);
        }

        [RoleAuth((int)enumsFile.adminRoles.Vendersinfo)]
        public async Task<IActionResult> VendorTable(int pageIndex, string name, int regionId)
        {
            var data = _partnersRepo.GetVendors();
            if (name != null)
            {
                data = data.Where(x => x.BusinessName.ToUpper().Contains(name.ToUpper()));
            }
            if (regionId != 0)
            {
                data = data.Where(x => x.regionId == regionId);
            }

            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            var pageSize = 4;
            data = data.OrderBy(x => x.BusinessName);
            return PartialView("_Vendorstable", await PaginatedList<VendorFormViewModel>.CreateAsync(data, pageIndex, pageSize));
        }

        [RoleAuth((int)enumsFile.adminRoles.Vendersinfo)]
        public IActionResult AddVendors()
        {
            ViewBag.AdminName = GetAdminName();
            var data = new VendorFormViewModel()
            {
                Healthprofessionaltypes = _partnersRepo.healthprofessionaltypes(),
                regions = _partnersRepo.regions()
            };
            return View(data);
        }

        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Vendersinfo)]
        public IActionResult AddVendors(VendorFormViewModel obj)
        {

            if (_loginRepo.isEmailAvailable(obj.Email))
            {
                _noty.Error("Email Already Registerd");
                obj.Healthprofessionaltypes = _partnersRepo.healthprofessionaltypes();

                return View(obj);
            }

            bool x = _partnersRepo.AddVendors(obj);

            if (x)
            {
                _noty.Success("Created Vendor");
            }
            else
            {
                _noty.Error("Something Went Wrong.");
            }

            return RedirectToAction("Partners");
        }

        [RoleAuth((int)enumsFile.adminRoles.Vendersinfo)]
        public IActionResult UpdateVendors(int id)
        {
            ViewBag.AdminName = GetAdminName();
            var data = _partnersRepo.UpdateVendors(id);
            return View(data);
        }
        [HttpPost]
        [RoleAuth((int)enumsFile.adminRoles.Vendersinfo)]
        public IActionResult UpdateVendors(VendorFormViewModel obj, int id)
        {
            ViewBag.AdminName = GetAdminName();

            if(_partnersRepo.GetVendorEmail(id) != obj.Email && _loginRepo.isEmailAvailable(obj.Email))
            {
                _noty.Error("Email is already registered.");
                obj.Healthprofessionaltypes = _partnersRepo.healthprofessionaltypes();
                return View(obj);
            }

            bool x = _partnersRepo.UpdateVendors(obj, id);
            if (x)
            {
                _noty.Success("Updated Vendor");
            }
            else
            {
                _noty.Error("Something Went Wrong.");
            }
            return RedirectToAction("Partners");
        }

        [RoleAuth((int)enumsFile.adminRoles.Vendersinfo)]
        public IActionResult DeleteVendors(int id)
        {
            bool x = _partnersRepo.DeleteVendors(id);

            if (x)
            {
                _noty.Success("Vendor deleted");
            }
            else
            {
                _noty.Error("Something Went Wrong.");
            }

            return RedirectToAction("Partners");
        }
    }
}
