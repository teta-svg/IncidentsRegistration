using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Decision")]
[Index("IdIncident", Name = "UQ__Decision__0464F6651789835E", IsUnique = true)]
public partial class Decision
{
    [Key]
    [Column("ID_Decision")]
    public int IdDecision { get; set; }

    [Column("ID_Incident")]
    public int IdIncident { get; set; }

    [Column("ID_Response_team")]
    public int? IdResponseTeam { get; set; }

    [Column("Legal_basis")]
    [StringLength(50)]
    [Unicode(false)]
    public string LegalBasis { get; set; } = null!;

    [Column("Date_of_adoption")]
    public DateOnly DateOfAdoption { get; set; }

    [Column("Decision_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string DecisionType { get; set; } = null!;

    [InverseProperty("IdDecisionNavigation")]
    public virtual CriminalCase? CriminalCase { get; set; }

    [ForeignKey("IdIncident")]
    [InverseProperty("Decision")]
    public virtual Incident IdIncidentNavigation { get; set; } = null!;

    [ForeignKey("IdResponseTeam")]
    [InverseProperty("Decisions")]
    public virtual ResponseTeam? IdResponseTeamNavigation { get; set; }

    [InverseProperty("IdDecisionNavigation")]
    public virtual TerritorialTransfer? TerritorialTransfer { get; set; }
}
