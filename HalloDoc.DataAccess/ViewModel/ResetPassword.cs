using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel
{
    public class ResetPassword
    {
        public string? Id { get; set; }

        [Required]
        [Column("Password")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password doest not match.")]
        [Column("confirmPassword")]
        public string? ConfirmPassword { get; set; }

        public string? email { get; set; }

    }
}
