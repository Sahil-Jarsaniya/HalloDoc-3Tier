using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class CreateShift
    {
        public int Shiftid { get; set; }
        public int Regionid { get; set; }

        public int Physicianid { get; set; }

        public DateOnly Startdate { get; set; }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EnddTime { get; set; }

        public bool Isrepeat { get; set; }

        public string? Weekdays { get; set; }

        public int? Repeatupto { get; set; }

        public IEnumerable<Region> Regions { get; set; }

        public IEnumerable<CheckBoxData> Days { get; set; }     

    }
}
