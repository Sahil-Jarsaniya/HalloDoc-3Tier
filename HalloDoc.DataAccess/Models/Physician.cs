﻿using System;
using System.Collections.Generic;

namespace HalloDoc.DataAccess.Models;

public partial class Physician
{
    public int Physicianid { get; set; }

    public string? Aspnetuserid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Medicallicense { get; set; }

    public string? Photo { get; set; }

    public string? Adminnotes { get; set; }

    public bool? Isagreementdoc { get; set; }

    public bool? Isbackgrounddoc { get; set; }

    public bool? Istrainingdoc { get; set; }

    public bool? Isnondisclosuredoc { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? Regionid { get; set; }

    public string? Zip { get; set; }

    public string? Altphone { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public short? Status { get; set; }

    public string Businessname { get; set; } = null!;

    public string Businesswebsite { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public int? Roleid { get; set; }

    public string? Npinumber { get; set; }

    public bool? Islicensedoc { get; set; }

    public string? Signature { get; set; }

    public bool? Iscredentialdoc { get; set; }

    public bool? Istokengenerate { get; set; }

    public string? Syncemailaddress { get; set; }

    public virtual AspNetUser? Aspnetuser { get; set; }

    public virtual AspNetUser CreatedbyNavigation { get; set; } = null!;

    public virtual AspNetUser? ModifiedbyNavigation { get; set; }

    public virtual ICollection<Physicianlocation> Physicianlocations { get; set; } = new List<Physicianlocation>();

    public virtual ICollection<Physiciannotification> Physiciannotifications { get; set; } = new List<Physiciannotification>();

    public virtual ICollection<Physicianregion> Physicianregions { get; set; } = new List<Physicianregion>();

    public virtual ICollection<ProviderFile> ProviderFiles { get; set; } = new List<ProviderFile>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Requeststatuslog> RequeststatuslogPhysicians { get; set; } = new List<Requeststatuslog>();

    public virtual ICollection<Requeststatuslog> RequeststatuslogTranstophysicians { get; set; } = new List<Requeststatuslog>();

    public virtual ICollection<Requestwisefile> Requestwisefiles { get; set; } = new List<Requestwisefile>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
