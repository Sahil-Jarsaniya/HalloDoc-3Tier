using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class ProviderTableViewModel
    {
        public int Physicianid { get; set; }

        public string? Aspnetuserid { get; set; }

        public string Firstname { get; set; } = null!;

        public string? Lastname { get; set; }

        public string Email { get; set; } = null!;

        public string? Mobile { get; set; }

        public string? Status { get; set; }

        public string onCallStatus { get; set; }

        public string? Roleid { get; set; }

        public bool isNotiOff { get; set; }
        public bool? isDeleted { get; set; }

        
    }
}
