using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class PatientChat
{
    public int Id { get; set; }

    public int PatientUserId { get; set; }

    public string Message { get; set; } = null!;

    public int ReqClientId { get; set; }

    public int SenderAccountType { get; set; }

    public DateTime CreateTime { get; set; }
}
