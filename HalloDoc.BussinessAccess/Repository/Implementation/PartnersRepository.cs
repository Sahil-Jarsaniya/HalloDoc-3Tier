using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.PartnersMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class PartnersRepository : IPartnersRepository
    {
        private readonly ApplicationDbContext _db;

        public PartnersRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Healthprofessionaltype> healthprofessionaltypes()
        {
            return _db.Healthprofessionaltypes;
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

        public bool AddVendors(VendorFormViewModel obj)
        {
            try
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

                return true;
            }
            catch
            {
                return false;
            }
        }

        public VendorFormViewModel UpdateVendors(int id)
        {
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
            return data;
        }

        public bool UpdateVendors(VendorFormViewModel obj, int id)
        {
            try
            {
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

                return true;    
            }
            catch
            {
                return false;   
            }
        }

        public bool DeleteVendors(int id)
        {
            try
            {
                var vendor = _db.Healthprofessionals.Where(x => x.Vendorid == id).FirstOrDefault();
                vendor.Isdeleted = true;
                _db.Healthprofessionals.Update(vendor);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
