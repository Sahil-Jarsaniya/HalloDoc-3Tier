using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class PayRate
{
    public int Id { get; set; }

    public int PhysicianId { get; set; }

    public int CategoryId { get; set; }

    public int PayRate1 { get; set; }

    public virtual PayrateCategory Category { get; set; } = null!;

    public virtual Physician Physician { get; set; } = null!;
}
