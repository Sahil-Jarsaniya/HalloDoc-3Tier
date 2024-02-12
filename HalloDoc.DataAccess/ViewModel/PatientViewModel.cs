using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.DataAccess.ViewModel
{
    public class PatientViewModel
    {
        //public required AspNetUser aspNetUsers {  get; set; }

        //public required Request request { get; set; }
        //public required Requestclient requestClient { get; set; }

        [Required]
        [Column("notes")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        [Column("firstname")]
        [StringLength(100)]
        public string? Firstname { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        public string? Email { get; set; }

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

        public IFormFile formFile { get; set; }
    }
}
