using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class CloseCaseViewModel
    {
        public int ReqClientId { get; set; }
        public string? confirmationNo { get; set; }

        public int? status { get; set; }

        [Required]
        [Column("firstname")]
        [StringLength(100)]
        public required string Firstname { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public required string Lastname { get; set; }

        [Column("strmonth")]
        [StringLength(20)]
        public string? Strmonth { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        public required string Email { get; set; }

        public IEnumerable<PatientDocumentViewModel> PatientDocumentViewModel { get; set; }
    }
}
