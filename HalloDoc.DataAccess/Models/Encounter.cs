using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.DataAccess.Models;

public partial class Encounter
{
    public int EncounterId { get; set; }

    public int Requestid { get; set; }

    [Required(ErrorMessage = "Please Enter Name")]
    [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid First Name")]
    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [RegularExpression(@"[a-zA-Z]*", ErrorMessage = "Invalid Last Name")]
    [Column("lastname")]
    [StringLength(100)]
    public string? LastName { get; set; }

    public string? Location { get; set; }

    public string? Strmonth { get; set; }

    public int? Intyear { get; set; }

    public int? Intdate { get; set; }

    public DateTime? Servicedate { get; set; }

    public string? Phonenumber { get; set; }

    [Required(ErrorMessage = "Field can't be empty")]
    [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    public string? PresentIllnessHistory { get; set; }

    public string? MedicalHistory { get; set; }

    public string? Medications { get; set; }

    public string? Allergies { get; set; }

    public string? Temperature { get; set; }

    public string? HeartRate { get; set; }

    public string? RespirationRate { get; set; }

    public string? BloodPressureSystolic { get; set; }

    public string? BloodPressureDiastolic { get; set; }

    public string? OxygenLevel { get; set; }

    public string? Pain { get; set; }

    public string? Heent { get; set; }

    public string? Cardiovascular { get; set; }

    public string? Chest { get; set; }

    public string? Abdomen { get; set; }

    public string? Extremities { get; set; }

    public string? Skin { get; set; }

    public string? Neuro { get; set; }

    public string? Other { get; set; }

    public string? Diagnosis { get; set; }

    public string? TreatmentPlan { get; set; }

    public string? MedicationsDispensed { get; set; }

    public string? Procedures { get; set; }

    public string? FollowUp { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? Isfinalized { get; set; }

    public DateTime? FinalizedDate { get; set; }

    public virtual Request Request { get; set; } = null!;
}
