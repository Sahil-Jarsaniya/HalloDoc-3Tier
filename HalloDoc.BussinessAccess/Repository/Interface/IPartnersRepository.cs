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
    }
}
