using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class BiWeeklySheet
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public int? Physicianid { get; set; }

    public bool? Weekend { get; set; }

    public int? NumberOfHousecall { get; set; }

    public int? NumberOfPhoneConsult { get; set; }

    public bool? IsFinal { get; set; }

    public TimeSpan? OnCallStatus { get; set; }

    public TimeSpan? Totalhour { get; set; }
}
