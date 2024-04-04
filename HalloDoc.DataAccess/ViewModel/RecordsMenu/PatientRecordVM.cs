using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.RecordsMenu
{
    public class PatientRecordVM
    {
        public string? Client { get; set; } 

        public DateTime createDate { get; set; }
        public string? confirmation { get; set; }

        public string? providerName { get; set; }

        public DateTime? ConcludedDate { get; set; }

        public string status { get; set; }

        public bool isFinalize { get; set; }        
        public string finalReport { get; set; }

        public int reqId { get; set; }
        public int reqClientId { get; set; }

    }
}
