using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class PayrateCategory
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<PayRate> PayRates { get; set; } = new List<PayRate>();
}
