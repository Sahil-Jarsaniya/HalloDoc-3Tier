using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class closeReqViewModel
    {
        public int reqClientId { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? ConciergePhonenumber { get; set; }

        public string? BusinessPhonenumber { get; set; }

        public string? FamilyPhonenumber { get; set; }

        public string? Email { get; set; }

        public int? Regionid { get; set; }

        public short Status { get; set; }

        public int? Physicianid { get; set; }

        public DateTime Createddate { get; set; }

        public string? Notes { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public int? reqTypeId { get; set; }

        public string? physicianName { get; set; }
    }
}
