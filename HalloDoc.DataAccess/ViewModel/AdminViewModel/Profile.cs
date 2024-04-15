using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class Profile
    {
        public int Adminid { get; set; }

        public string? Password { get; set; }
        public string? UserName { get; set; } = null!;

        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Column("firstname")]
        [StringLength(100)]
        public string? Firstname { get; set; } = null!;

        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid Last Name")]
        [Column("lasttname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("Email")]
        [StringLength(50)]
        public string? Email { get; set; } = null!;

        [NotMapped]
        [Compare("Email", ErrorMessage ="Email does not match.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public string? ConfirmEmail { get; set; } = null!;

        public string? Mobile { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public int? Regionid { get; set; }

        public string? Zip { get; set; }

        public string? Modifiedby { get; set; }

        public DateTime? Modifieddate { get; set; }

        public short? Status { get; set; }

        public int? Roleid { get; set; }

        public string? Altphone { get; set; }

        public IEnumerable<CheckBoxData>? Region { get; set; }
        public IEnumerable<Role>? Roles { get; set; }
        public IEnumerable<PhysicianStatus>? Statues{ get; set; }
    }
}
