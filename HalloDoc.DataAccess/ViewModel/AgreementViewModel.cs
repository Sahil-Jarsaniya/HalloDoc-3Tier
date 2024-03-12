using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel
{
    public class AgreementViewModel
    {
        public int Requestclientid { get; set; }

        public string Firstname { get; set; } = null!;

        public string? Lastname { get; set; }

        public string? CancelNote { get; set; }
    }
}
