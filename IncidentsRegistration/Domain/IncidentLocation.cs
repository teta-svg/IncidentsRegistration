using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Incident_Location")]
public partial class IncidentLocation
{
    [Key]
    [Column("ID_Incident_Location")]
    public int IdIncidentLocation { get; set; }

    [Column("ID_Incident")]
    public int IdIncident { get; set; }

    [Column("ID_Location")]
    public int IdLocation { get; set; }

    [ForeignKey("IdIncident")]
    [InverseProperty("IncidentLocations")]
    public virtual Incident IdIncidentNavigation { get; set; } = null!;

    [ForeignKey("IdLocation")]
    [InverseProperty("IncidentLocations")]
    public virtual Location IdLocationNavigation { get; set; } = null!;
}
