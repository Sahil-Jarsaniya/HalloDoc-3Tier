using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class RequestStatus
{
    public int StatusId { get; set; }

    public string Status { get; set; } = null!;
}
