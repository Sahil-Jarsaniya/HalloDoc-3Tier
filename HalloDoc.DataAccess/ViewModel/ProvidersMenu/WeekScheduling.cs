using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class WeekScheduling
    {
        public DateOnly Selecteddate { get; set; }

        public required IEnumerable<Physician> physicians { get; set; }
        public IEnumerable <Shift>? shifts { get; set; }  
        public IEnumerable<Shiftdetail>? shiftdetails { get; set; }  
        public IEnumerable<DayScheduling>? daySchedulings { get; set; }  
    }
}
