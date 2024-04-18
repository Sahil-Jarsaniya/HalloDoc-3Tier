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
    public class ConciergeViewModel
    {
        [Required(ErrorMessage = "This field can't be empty.")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Column("ConciergeFirstname")]
        [StringLength(100)]
        public string? ConciergeFirstname { get; set; }

        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Column("ConciergeLastname")]
        [StringLength(100)]
        public string? ConciergeLastname { get; set; }

        [Column("ConciergePhonenumber")]
        [StringLength(23)]
        public string? ConciergePhonenumber { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("ConciergeEmail")]
        [StringLength(50)]
        public string? ConciergeEmail { get; set; }

        [Column("ConciergeProperty")]
        [StringLength(50)]
        public string? ConciergeProperty { get; set; }

        [Column("Conciergestreet")]
        [StringLength(100)]
        public string? ConciergeStreet { get; set; }

        [Column("Conciergecity")]
        [StringLength(100)]
        public string? ConciergeCity { get; set; }

        [Column("Conciergestate")]
        [StringLength(100)]
        public string? ConciergeState { get; set; }

        [Column("Conciergezipcode")]
        [StringLength(10)]
        public string? ConciergeZipcode { get; set; }

        [Required(ErrorMessage = "This field can't be empty.")]
        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "This field can't be empty.")]
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

        public string? ConciergeCountryCode { get; set; }

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
        public int regionId { get; set; }

        public string? countryCode { get; set; }
        public List<Region>? Regions { get; set; }
    }
}