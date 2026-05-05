using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Incident")]
public partial class Incident
{
    [Key]
    [Column("ID_Incident")]
    public int IdIncident { get; set; }

    [Column("ID_Response_team")]
    public int? IdResponseTeam { get; set; }

    [Column("Type_of_incident")]
    [StringLength(50)]
    [Unicode(false)]
    public string TypeOfIncident { get; set; } = null!;

    [Column("Registration_time")]
    public TimeOnly RegistrationTime { get; set; }

    [Column("Registration_date")]
    public DateOnly RegistrationDate { get; set; }

    [InverseProperty("IdIncidentNavigation")]
    public virtual Decision? Decision { get; set; }

    [ForeignKey("IdResponseTeam")]
    [InverseProperty("Incidents")]
    public virtual ResponseTeam? IdResponseTeamNavigation { get; set; }

    [InverseProperty("IdIncidentNavigation")]
    public virtual ICollection<IncidentLocation> IncidentLocations { get; set; } = new List<IncidentLocation>();

    [InverseProperty("IdIncidentNavigation")]
    public virtual ICollection<SubjectRole> SubjectRoles { get; set; } = new List<SubjectRole>();
}
