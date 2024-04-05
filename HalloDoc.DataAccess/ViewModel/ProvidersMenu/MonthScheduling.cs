using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class MonthScheduling
    {
        public DateOnly Selecteddate { get; set; }
        public required IEnumerable<Physician> physicians { get; set; }
        public required IEnumerable<DayScheduling> DaySchedulings{ get; set; }
    }
}
