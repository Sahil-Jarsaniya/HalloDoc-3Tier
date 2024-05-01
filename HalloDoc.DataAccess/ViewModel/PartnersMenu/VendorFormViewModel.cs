using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.DataAccess.Models;

namespace HalloDoc.DataAccess.ViewModel.PartnersMenu
{
    public class VendorFormViewModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        [Column("BusinessName")]
        [StringLength(100)]
        public string BusinessName { get; set; }

        [Column("FaxNumber")]
        [StringLength(100)]
        public string FaxNumber { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }
        
        [Column("BusinessContact")]
        [StringLength(23)]
        public string? BusinessContact { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }

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

        public int vendorId { get; set; }

        public string? CountryFlag { get; set; }
        public int professionTypeId { get; set; }
        public string? professionName { get; set; }

        public int? regionId { get; set; }

        public IEnumerable<Healthprofessionaltype>?  Healthprofessionaltypes { get; set; }
        public IEnumerable<Region>? regions { get; set; }
    }
}
