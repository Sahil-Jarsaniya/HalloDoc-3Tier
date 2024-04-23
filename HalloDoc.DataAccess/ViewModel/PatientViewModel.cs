using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using HalloDoc.DataAccess.Models;

namespace HalloDoc.DataAccess.ViewModel
{
    public class PatientViewModel
    {
        //public required AspNetUser aspNetUsers {  get; set; }

        //public required Request request { get; set; }
        //public required Requestclient requestClient { get; set; }
        [Required(ErrorMessage = "This field can't be empty.")]
        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Column("firstname")]
        [StringLength(100)]
        public string Firstname { get; set; }

        [Column("lastname")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid Last Name")]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Required]
        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        public string? countryCode { get; set; } = "+91";

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }


        [Column("Password")]
        [StringLength(100)]
        public  string? Password { get; set; }

       /* [NotMapped]*/ // Does not effect with your database
        //[Compare("Password", ErrorMessage ="Password doest not match.")]
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

        public int regionId { get; set; }

        [Column("zipcode")]
        [StringLength(10)]
        public string? Zipcode { get; set; }

        public IFormFile? formFile { get; set; }

        public string? Relationname { get; set; }

        public List<Region>? Regions { get; set; }
    }
}
