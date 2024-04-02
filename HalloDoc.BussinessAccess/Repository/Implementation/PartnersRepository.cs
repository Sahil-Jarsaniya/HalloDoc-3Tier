using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.ViewModel.PartnersMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class PartnersRepository: IPartnersRepository
    {
        private readonly ApplicationDbContext _db;

        public PartnersRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public PartnersViewModel Partners()
        {
            
            var data = new PartnersViewModel()
            {  
                regions = _db.Regions,
            };
            return data;
        }
        public IQueryable<VendorFormViewModel> GetVendors()
        {
            var vendorData = (from t1 in _db.Healthprofessionals
                              join t2 in _db.Healthprofessionaltypes on t1.Profession equals t2.Healthprofessionalid
                              where t1.Isdeleted != true
                              select new VendorFormViewModel()
                              {
                                  vendorId = t1.Vendorid,
                                  professionName = t2.Professionname,
                                  BusinessName = t1.Vendorname,
                                  BusinessContact = t1.Businesscontact,
                                  Email = t1.Email,
                                  Phonenumber = t1.Phonenumber,
                                  FaxNumber = t1.Faxnumber,
                                  regionId = t1.Regionid
                              });
            return vendorData;  
        }
    }
}
