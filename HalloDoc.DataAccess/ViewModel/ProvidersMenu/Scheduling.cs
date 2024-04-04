using HalloDoc.DataAccess.Models;
using HalloDoc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class Scheduling
    {
        public DateOnly Date { get; set; }

        public required IEnumerable<Region> Regions { get; set; }

        public PaginatedList<RequestedShiftVM>? requestedShiftVMs { get; set; }

        public IEnumerable<ProviderOnCall>? ProviderOnCall { get; set; }    
        public IEnumerable<ProviderOnCall>? ProviderOffDuty { get; set; }    
    }
}
