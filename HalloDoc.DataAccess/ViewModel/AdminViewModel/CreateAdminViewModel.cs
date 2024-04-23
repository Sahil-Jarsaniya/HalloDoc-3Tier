using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required(ErrorMessage = "Please Enter Name")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        public string Firstname { get; set; } = null!;

        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid Last Name")]
        public string? Lastname { get; set; }

        [Column("Email")]
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [Compare("Email", ErrorMessage = "Email doest not match.")]
        public string ConfirmEmail { get; set; } = null!;

        [Required]
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

        public string? CountryFlag { get; set; }

        public IEnumerable<Region>? Regions { get; set; }
        public IEnumerable<CheckBoxData>? CheckedRegion { get; set; }

        [Required(ErrorMessage ="Please Select Region.")]
        public List<int>? SelectedRegion { get; set; }
        public IEnumerable<Role> Rolemenus { get; set; }

    }
}
