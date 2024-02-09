﻿using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class Business
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? Regionid { get; set; }

    public string? Zipcode { get; set; }

    public string? Phonenumber { get; set; }

    public string? Faxnumber { get; set; }

    public bool? Isrefistered { get; set; }

    public string? Createdby { get; set; }

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime Modifieddate { get; set; }

    public short? Status { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Ip { get; set; }

    public virtual Aspnetuser1? CreatedbyNavigation { get; set; }

    public virtual Aspnetuser1? ModifiedbyNavigation { get; set; }

    public virtual Region? Region { get; set; }

    public virtual ICollection<Requestbusiness> Requestbusinesses { get; set; } = new List<Requestbusiness>();
}
