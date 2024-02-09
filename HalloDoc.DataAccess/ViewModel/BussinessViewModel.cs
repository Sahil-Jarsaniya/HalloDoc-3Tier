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
        [Required]
        [Column("bussinessFirstname")]
        [StringLength(100)]
        public string? bussinessFirstname { get; set; }

        [Column("bussinessLastname")]
        [StringLength(100)]
        public string? bussinessLastname { get; set; }

        [Column("BussinessPhonenumber")]
        [StringLength(23)]
        public string? bussinessPhonenumber { get; set; }

        [Column("bussinessEmail")]
        [StringLength(50)]
        public string? bussinessEmail { get; set; }

        [Column("bussinessProperty")]
        [StringLength(50)]
        public string? bussinessProperty { get; set; }

        [Column("CaseNumber")]
        [StringLength(50)]
        public string? CaseNumber { get; set; }



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
    }
}
