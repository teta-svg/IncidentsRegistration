using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Criminal_case")]
[Index("IdDecision", Name = "UQ__Criminal__ED013922A22B6049", IsUnique = true)]
public partial class CriminalCase
{
    [Key]
    [Column("Сase_number")]
    public int СaseNumber { get; set; }

    [Column("ID_Decision")]
    public int IdDecision { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Qualification { get; set; } = null!;

    [Column("Date_of_initiation")]
    public DateOnly DateOfInitiation { get; set; }

    [ForeignKey("IdDecision")]
    [InverseProperty("CriminalCase")]
    public virtual Decision IdDecisionNavigation { get; set; } = null!;
}
