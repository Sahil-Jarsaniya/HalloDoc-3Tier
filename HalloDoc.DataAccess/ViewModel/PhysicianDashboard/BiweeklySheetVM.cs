using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.PhysicianDashboard
{
    public class DateVM
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public List<BiweeklySheetVM>? biweeklySheetVMs { get; set; }

        public List<BiWeeklyReciept>? biWeeklyReciepts { get; set; }
    }

    public class BiweeklySheetVM
    {
        public DateOnly Date { get; set; }

        public TimeSpan OnCallStatus { get; set; }
        public TimeSpan TotalHour { get; set; }

        public bool Weekend { get; set; }

        public int NumberOfHouseCall { get; set; }

        public int NumberOfPhoneConsults { get; set; }
    }

    public class BiWeeklyReciept
    {
        public DateOnly Date { get; set; }

        public required string item { get; set; }

        public int amount { get; set; }

        public string? bill { get; set; }
    }
}
