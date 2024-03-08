using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class SendOrderViewModel
    {
        public IEnumerable<Healthprofessionaltype>? Healthprofessionaltype { get; set; }
        public int reqClientId { get; set; }    
        public int ProfessionTypeId { get; set; }
        public int ProfessionalId { get; set; }
        public string? ProfessionalPhone { get; set; }
        public string? FaxNumber { get; set; }
        public string? email { get; set; }
        public string? OrderDetail { get; set; }
        public int noOfRefill { get; set; }

        public int? status { get; set; }
    }
}
