using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.RecordsMenu
{
    public class PatientHistoryVM
    {
        public int? reqId { get; set; }    
        public string? firstName { get; set; }  
        public string? lastName { get; set; }

        public string? email { get; set; }

        public string? phoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
