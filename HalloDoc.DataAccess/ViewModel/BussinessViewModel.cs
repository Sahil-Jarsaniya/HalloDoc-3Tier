using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.DataAccess.Models;

namespace HalloDoc.DataAccess.ViewModel
{
    public class BussinessViewModel
    {
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        [Column("bussinessFirstname")]
        [StringLength(100)]
        public string? bussinessFirstname { get; set; }

        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid Last Name")]
        [Column("bussinessLastname")]
        [StringLength(100)]
        public string? bussinessLastname { get; set; }

        [Column("BussinessPhonenumber")]
        [StringLength(23)]
        public string? bussinessPhonenumber { get; set; }

        public string? businessCountryFlag { get; set; }    

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("bussinessEmail")]
        [StringLength(50)]
        public string? bussinessEmail { get; set; }

        [Column("bussinessProperty")]
        [StringLength(50)]
        public string? bussinessProperty { get; set; }

        [Column("CaseNumber")]
        [StringLength(50)]
        public string? CaseNumber { get; set; }

        [Required(ErrorMessage = "You have to select State.")]
        public int regionId { get; set; }

        [Required(ErrorMessage = "Please Enter Note")]
        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [Column("firstname")]
        [StringLength(100)]
        public string? Firstname { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }
        public string? patientCountryFlag { get; set; }    

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public string? Email { get; set; }

        [Column("Password")]
        [StringLength(100)]
        public string? Password { get; set; }


        [Column("confirmPassword")]
        [StringLength(100)]
        public string? confirmPassword { get; set; }

        [Column("strmonth")]
        [StringLength(20)]
        public string? Strmonth { get; set; }

        [Column("street")]
        [StringLength(100)]
        public string? Street { get; set; }

        [Column("city")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [StringLength(100)]
        public string? State { get; set; }

        [Column("zipcode")]
        [StringLength(10)]
        public string? Zipcode { get; set; }

        public string? countryCode { get; set; }
        public List<Region>? Regions { get; set; }
    }
}
