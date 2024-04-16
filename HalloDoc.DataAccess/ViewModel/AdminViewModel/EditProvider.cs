using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class EditProvider
    {
        public int AdminId { get; set; }

        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
        public int Physicianid { get; set; }

        public string? Aspnetuserid { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Column("firstname")]
        [StringLength(100)]
        public string Firstname { get; set; } = null!;
        [Column("lastname")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid Last Name")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        public string? Mobile { get; set; }

        public string? Medicallicense { get; set; }

        public string? Photo { get; set; }

        public string? Adminnotes { get; set; }



        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }

        public int? Regionid { get; set; }

        public string? Zip { get; set; }

        public string? Altphone { get; set; }

        public string? Createdby { get; set; }

        public DateTime Createddate { get; set; }

        public string? Modifiedby { get; set; }

        public DateTime? Modifieddate { get; set; }

        public short? Status { get; set; }

        [Required]
        public string Businessname { get; set; } = null!;

        [Required]
        public string Businesswebsite { get; set; } = null!;

        public bool? Isdeleted { get; set; }

        public int? Roleid { get; set; }

        public string? Npinumber { get; set; }

        public string? Signature { get; set; }

        public string? Syncemailaddress { get; set; }

        public bool Isagreementdoc { get; set; }

        public bool Isbackgrounddoc { get; set; }

        public bool Istrainingdoc { get; set; }

        public bool Islicensedoc { get; set; }

        public bool Isnondisclosuredoc { get; set; }

        public IFormFile? agreementdoc { get; set; }

        public IFormFile? backgrounddoc { get; set; }

        public IFormFile? trainingdoc { get; set; }

        public IFormFile? licensedoc { get; set; }

        public IFormFile? nondisclosuredoc { get; set; }

        public IFormFile? PhySign { get; set; }
        public IFormFile? PhyPhoto { get; set; }

        public IEnumerable<CheckBoxData>? Region { get; set; }
        public IEnumerable<Role>? Role { get; set; }
        public IEnumerable<PhysicianStatus>? Statuses { get; set; }
    }
}
