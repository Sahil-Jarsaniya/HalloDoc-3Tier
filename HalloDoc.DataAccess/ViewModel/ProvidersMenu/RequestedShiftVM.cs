using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class RequestedShiftVM
    {
        public int shiftDetailId { get; set; }
        public string? staff { get; set; }

        public string? Day { get; set; }

        public string? Time { get; set; }

        public string? Region { get; set; }
        public int RegionId { get; set; }
    }
}
