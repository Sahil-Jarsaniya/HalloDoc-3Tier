using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class Aspnetuser1
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Passwordhash { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public string? Ip { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Modifieddate { get; set; }

    public virtual ICollection<Aspnetuserrole1> Aspnetuserrole1s { get; set; } = new List<Aspnetuserrole1>();

    public virtual ICollection<Business> BusinessCreatedbyNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Business> BusinessModifiedbyNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Physician> PhysicianAspnetusers { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianModifiedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Shiftdetail> Shiftdetails { get; set; } = new List<Shiftdetail>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
