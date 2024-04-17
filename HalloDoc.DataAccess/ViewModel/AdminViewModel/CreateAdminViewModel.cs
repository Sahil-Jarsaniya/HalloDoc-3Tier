using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class CreateAdminViewModel
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
        public int Adminid { get; set; }

        public string? Aspnetuserid { get; set; }

        public string Firstname { get; set; } = null!;

        public string? Lastname { get; set; }

        public string Email { get; set; } = null!;

        public string? Mobile { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public int? Regionid { get; set; }

        public string? Zip { get; set; }

        public string? Altphone { get; set; }

        public string Createdby { get; set; } = null!;

        public DateTime Createddate { get; set; }

        public string? Modifiedby { get; set; }

        public DateTime? Modifieddate { get; set; }

        public short? Status { get; set; }

        public bool? Isdeleted { get; set; }

        public int? Roleid { get; set; }

        public IEnumerable<Region>? Regions { get; set; }
        public IEnumerable<CheckBoxData>? CheckedRegion { get; set; }

        public List<int>? SelectedRegion { get; set; }
        public IEnumerable<Role> Rolemenus { get; set; }

    }
}
