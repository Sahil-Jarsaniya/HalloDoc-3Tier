using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class AccountType
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
