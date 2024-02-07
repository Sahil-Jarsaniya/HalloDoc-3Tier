using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DataAccess.Models;

[PrimaryKey("Userid", "Name")]
[Table("aspnetuserroles")]
public partial class Aspnetuserrole1
{
    [Key]
    [Column("userid")]
    [StringLength(128)]
    public string Userid { get; set; } = null!;

    [Key]
    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserrole1s")]
    public virtual Aspnetuser1 User { get; set; } = null!;
}
