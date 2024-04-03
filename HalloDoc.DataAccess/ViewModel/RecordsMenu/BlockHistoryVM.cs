using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.RecordsMenu
{
    public class BlockHistoryVM
    {
        public string? PatientName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Notes { get; set; }
        public bool? isActive { get; set; }
        public int? ReqStatusId { get; set; }
        public int? ReqId { get; set; }
    }
}
