using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class ProviderFileType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ProviderFile> ProviderFiles { get; set; } = new List<ProviderFile>();
}
