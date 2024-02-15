using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.DataAccess.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    [Column("PasswordHash")]
    public string Password { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Ip { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<AspNetRole1> Roles { get; set; } = new List<AspNetRole1>();
}
