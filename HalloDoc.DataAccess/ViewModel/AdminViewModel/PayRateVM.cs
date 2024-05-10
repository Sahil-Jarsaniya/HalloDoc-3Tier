using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class PayRateVM
    {
        public int categoryId { get; set; }
        public required string category { get; set; }

        public int payrate { get; set; }

        public int physicianId { get; set; }
    }
}
