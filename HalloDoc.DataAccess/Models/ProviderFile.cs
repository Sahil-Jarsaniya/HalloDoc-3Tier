using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class ProviderFile
{
    public int FileId { get; set; }

    public int PhysicianId { get; set; }

    public string FileName { get; set; } = null!;

    public int FileType { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ProviderFileType FileTypeNavigation { get; set; } = null!;

    public virtual Physician Physician { get; set; } = null!;
}
