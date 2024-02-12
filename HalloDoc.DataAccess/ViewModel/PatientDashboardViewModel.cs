using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel
{
    public class PatientDashboardViewModel
    {
        public DateTime Createddate { get; set; }

        public short Status { get; set; }

        public string Filename { get; set; } = null!;

    }
}
