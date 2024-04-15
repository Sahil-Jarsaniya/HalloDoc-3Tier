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
    public class SendOrderViewModel
    {
        public IEnumerable<Healthprofessionaltype>? Healthprofessionaltype { get; set; }
        public int reqClientId { get; set; }

        [Required(ErrorMessage = "Select this option first")]
        public int ProfessionTypeId { get; set; }
        
        [Required(ErrorMessage = "Select this option first")]
        public int ProfessionalId { get; set; }
        [Required]
        public string? ProfessionalPhone { get; set; }
        [Required]
        public string? FaxNumber { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public string? email { get; set; }

        [Required]
        public string? OrderDetail { get; set; }

        [Required]
        public int noOfRefill { get; set; }

        public int? status { get; set; }
    }
}
