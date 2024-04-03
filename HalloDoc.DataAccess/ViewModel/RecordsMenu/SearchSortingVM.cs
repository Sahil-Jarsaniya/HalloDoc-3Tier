using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.RecordsMenu
{
    public class SearchSortingVM
    {
        public int? ReqStatus { get; set; }
        public int? ReqType { get; set; }

        public string? PatientName { get; set; }

        public DateTime Date1 { get; set; }    
        public DateTime Date2 { get; set; }    

        public string? ProviderName { get; set; }

        public string? Email { get; set;}
        public string? Phonenumber { get; set;}
    }
}
