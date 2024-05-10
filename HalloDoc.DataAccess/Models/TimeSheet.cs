using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class TimeSheet
{
    public int Id { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsSheetCreated { get; set; }

    public bool IsApproved { get; set; }

    public int? PhysicianId { get; set; }

    public bool? IsFinal { get; set; }

    public string? Status { get; set; }

    public int? Bonus { get; set; }

    public int? Total { get; set; }

    public string? AdminNote { get; set; }

    public virtual Physician? Physician { get; set; }
}
