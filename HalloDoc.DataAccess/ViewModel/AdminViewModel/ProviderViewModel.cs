using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class ProviderViewModel
    {
        

        public IEnumerable<ProviderTableViewModel>? providerTableViewModels { get; set; }


        public IEnumerable<Region>? Region { get; set; }

    }
}
