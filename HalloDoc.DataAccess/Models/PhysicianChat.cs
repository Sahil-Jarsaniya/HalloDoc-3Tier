using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class PhysicianChat
{
    public int Id { get; set; }

    public int PhysicianId { get; set; }

    public string Message { get; set; } = null!;

    public int ReqClientId { get; set; }

    public int SenderAccountType { get; set; }

    public DateTime CreateTime { get; set; }
}
