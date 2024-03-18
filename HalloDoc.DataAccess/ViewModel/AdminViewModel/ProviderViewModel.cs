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
        public int Physicianid { get; set; }

        public string? Aspnetuserid { get; set; }

        public string Firstname { get; set; } = null!;

        public string? Lastname { get; set; }

        public string Email { get; set; } = null!;

        public string? Mobile { get; set; }


        public int? Regionid { get; set; }


        public short? Status { get; set; }


        public int? Roleid { get; set; }


        //public IEnumerable<Region>? Region { get; set; }

    }
}
