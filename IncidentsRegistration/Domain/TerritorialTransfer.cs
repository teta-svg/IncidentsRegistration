using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Territorial_transfer")]
[Index("IdDecision", Name = "UQ__Territor__ED013922D5F90158", IsUnique = true)]
public partial class TerritorialTransfer
{
    [Key]
    [Column("ID_Transfer")]
    public int IdTransfer { get; set; }

    [Column("ID_Decision")]
    public int IdDecision { get; set; }

    [Column("Date_of_transfer")]
    public DateOnly DateOfTransfer { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Division { get; set; } = null!;

    [ForeignKey("IdDecision")]
    [InverseProperty("TerritorialTransfer")]
    public virtual Decision IdDecisionNavigation { get; set; } = null!;
}
