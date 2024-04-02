using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.PartnersMenu
{
    public class PartnersViewModel
    {
        public string? Name { get; set; }
        public int? RegionId { get; set; }

        public IQueryable<VendorFormViewModel>? Vendors { get; set; }
        public required IEnumerable<Region> regions { get; set; }
    }
}
