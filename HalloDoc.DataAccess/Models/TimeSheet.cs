using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class TimeSheet
{
    public int Id { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsSheetCreated { get; set; }

    public bool IsReceiptCreated { get; set; }

    public int? PhysicianId { get; set; }

    public virtual Physician? Physician { get; set; }
}
