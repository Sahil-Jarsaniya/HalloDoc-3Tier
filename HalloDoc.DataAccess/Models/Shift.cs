﻿using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class Shift
{
    public int Shiftid { get; set; }

    public int Physicianid { get; set; }

    public DateOnly Startdate { get; set; }

    public bool Isrepeat { get; set; }

    public string? Weekdays { get; set; }

    public int? Repeatupto { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Ip { get; set; }

    public virtual Aspnetuser1 CreatedbyNavigation { get; set; } = null!;

    public virtual Physician Physician { get; set; } = null!;

    public virtual ICollection<Shiftdetail> Shiftdetails { get; set; } = new List<Shiftdetail>();
}
