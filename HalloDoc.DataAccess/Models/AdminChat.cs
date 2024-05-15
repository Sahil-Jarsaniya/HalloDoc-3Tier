using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class AdminChat
{
    public int Id { get; set; }

    public int AdminId { get; set; }

    public string Message { get; set; } = null!;

    public int ReqClientId { get; set; }

    public DateTime CreateTime { get; set; }

    public int SenderAccountType { get; set; }
}
