using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class BiWeeklyReceipt
{
    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public string? Item { get; set; }

    public int? Amount { get; set; }

    public string? Bill { get; set; }

    public int Physicianid { get; set; }

    public virtual Physician Physician { get; set; } = null!;
}
