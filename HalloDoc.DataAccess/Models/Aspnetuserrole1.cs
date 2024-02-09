using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class Aspnetuserrole1
{
    public string Userid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual Aspnetuser1 User { get; set; } = null!;
}
