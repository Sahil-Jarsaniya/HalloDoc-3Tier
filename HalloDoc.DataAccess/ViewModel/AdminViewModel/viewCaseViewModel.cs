using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class viewCaseViewModel
    {
        public int Requestclientid { get; set; }

        public int? Requestid { get; set; }

        public string Firstname { get; set; } = null!;

        public string? Lastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? Address { get; set; }

        public int? Regionid { get; set; }

        public string? Notes { get; set; }

        public string? Email { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public String confirmationNumber { get; set; }

        public int? status { get; set; }
    }
}
