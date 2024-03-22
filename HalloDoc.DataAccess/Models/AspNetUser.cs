using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Ip { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Admin> AdminAspnetusers { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminModifiedbyNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Physician> PhysicianAspnetusers { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianCreatedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<AspNetRole1> Roles { get; set; } = new List<AspNetRole1>();
}
