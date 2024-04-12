using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.DataAccess.ViewModel
{
    public class FamilyViewModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Column("familyFirstname")]
        [StringLength(100)]
        public string? FamilyFirstname { get; set; }

        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid Last Name")]
        [Column("bussinessLastname")]
        [StringLength(100)]
        public string? FamilyLastname { get; set; }

        [Column("BussinessPhonenumber")]
        [StringLength(23)]
        public string? FamilyPhonenumber { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("bussinessEmail")]
        [StringLength(50)]
        public string? FamilyEmail { get; set; }

        [Column("RelationWithPatient")]
        [StringLength(50)]
        public string? RelationWithPatient { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [Column("firstname")]
        [StringLength(100)]
        public string Firstname { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }

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

        public IFormFile? formFile { get; set; }
    }
}
