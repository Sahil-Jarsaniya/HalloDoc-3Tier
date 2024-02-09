﻿using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class AspNetRole1
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
