using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Location")]
public partial class Location
{
    [Key]
    [Column("ID_Location")]
    public int IdLocation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Region { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Settlement { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Street { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string? House { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string? Room { get; set; }

    [Column("Coordinate_X", TypeName = "decimal(10, 2)")]
    public decimal CoordinateX { get; set; }

    [Column("Coordinate_Y", TypeName = "decimal(10, 2)")]
    public decimal CoordinateY { get; set; }

    [InverseProperty("IdLocationNavigation")]
    public virtual ICollection<IncidentLocation> IncidentLocations { get; set; } = new List<IncidentLocation>();
}
