using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class CloseCaseViewModel
    {
        public int ReqClientId { get; set; }
        public string? confirmationNo { get; set; }

        public int? status { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [Column("firstname")]
        [StringLength(100)]
        public required string Firstname { get; set; }

        [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
        [StringLength(100)]
        public required string Lastname { get; set; }

        [Column("strmonth")]
        [StringLength(20)]
        public string? Strmonth { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public required string Email { get; set; }

        public string? note { get; set; }

        public IFormFile? fileName { get; set; }

        public IEnumerable<PatientDocumentViewModel> PatientDocumentViewModel { get; set; }

    }
}
