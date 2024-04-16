using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel
{
    public class login
    {
        [Required(ErrorMessage ="Please Enter username.")]
        public required string Username { get; set; }

        [Required(ErrorMessage ="Please Enter Password.")]
        public string Password { get; set; }
    }
}
