using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class CreateShift
    {
        public int Shiftid { get; set; }
        public int ShiftDetailId { get; set; }

        [Required]
        public int Regionid { get; set; }
        [Required]
        public int Physicianid { get; set; }
        public string? PhysicianName { get; set; }
        [Required]
        public DateOnly Startdate { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }

        public bool Isrepeat { get; set; }

        public string? Weekdays { get; set; }

        public int? Repeatupto { get; set; }
        public int? status { get; set; }

        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<Physicianregion>? phyRegions { get; set; }


        public IEnumerable<CheckBoxData> Days { get; set; }     

    }
}
