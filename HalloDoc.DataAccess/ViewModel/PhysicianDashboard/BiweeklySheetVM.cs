using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.PhysicianDashboard
{
    public class DateVM
    {
        public int timesheetId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; } 
        public bool isFinal { get; set; }

        public int physicianId { get; set; }    
        public List<BiweeklySheetVM>? biweeklySheetVMs { get; set; }

        public List<BiWeeklyRecieptVM>? biWeeklyReciepts { get; set; }

        public PayRateValueVM? payRates { get; set; }
    }

    public class PayRateValueVM
    {
        public int TotalHour { get; set; }
        public int weekend { get; set; }
        public int HouseCall { get; set; }
        public int PhoneConsult { get; set; }
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

    public class BiWeeklyRecieptVM
    {
        public DateOnly Date { get; set; }

        public required string item { get; set; }

        public int amount { get; set; }

        public IFormFile? bill { get; set; }

        public string? billName { get; set; }

        public bool isUploaded { get; set; }

        public int PhysicianId { get; set; }
    }

    public class sheetData
    {
        public DateOnly Date { get; set; }

        public int shift { get; set; }

        public int NightshiftWeekend { get; set; }

        public int HouseCall { get; set; }

        public int HousecallNightsWeekend { get; set; }

        public int phoneConsult { get; set; }

        public int phoneConsultNightsWeekend { get; set; }
        public int BatchTesting { get; set; }
    }
}
