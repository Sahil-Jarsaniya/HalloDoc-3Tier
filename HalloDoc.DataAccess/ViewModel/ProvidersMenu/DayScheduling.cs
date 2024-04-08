using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class DayScheduling
    {
        public string PhysicianName { get; set; }

        public int PhysicianId { get; set; }

        public int? status { get; set; }
        public int? Shiftid { get; set; }
        public int? shiftDetailId { get; set; }
        public int? regionId { get; set; }
        public DateOnly? Startdate { get; set; }

        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }

        public DateOnly? ShiftDate { get; set; }
        public DateOnly SelectedDate { get; set; }

        public IEnumerable<Physician> physicians { get; set; }
    }
}
