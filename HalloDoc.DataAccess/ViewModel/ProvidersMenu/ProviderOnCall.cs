using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class ProviderOnCall
    {
        public int? shiftDetailId { get; set; }
        public int? providerId { get; set; }

        public string? Name { get; set; }

        public string? profilePhoto { get; set; }

        public bool isOnDuty { get; set; }
    }
}
