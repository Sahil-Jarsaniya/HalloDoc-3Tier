using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.PartnersMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IPartnersRepository
    {
        public PartnersViewModel Partners();

        public IQueryable<VendorFormViewModel> GetVendors();

        public IEnumerable<Healthprofessionaltype> healthprofessionaltypes();

        public bool AddVendors(VendorFormViewModel obj);
        public VendorFormViewModel UpdateVendors(int id);
        public bool UpdateVendors(VendorFormViewModel obj,int id);
        public bool DeleteVendors(int id);
    }
}
