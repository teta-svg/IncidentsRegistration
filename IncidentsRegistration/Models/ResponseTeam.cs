using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Response_team")]
public partial class ResponseTeam
{
    [Key]
    [Column("ID_Response_team")]
    public int IdResponseTeam { get; set; }

    [Column("Name_team")]
    [StringLength(50)]
    [Unicode(false)]
    public string NameTeam { get; set; } = null!;

    [Column("Director_Last_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string DirectorLastName { get; set; } = null!;

    [Column("Director_First_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string DirectorFirstName { get; set; } = null!;

    [Column("Director_Patronymic")]
    [StringLength(50)]
    [Unicode(false)]
    public string? DirectorPatronymic { get; set; }

    [Column("Number_of_people")]
    public int NumberOfPeople { get; set; }

    [InverseProperty("IdResponseTeamNavigation")]
    public virtual ICollection<Decision> Decisions { get; set; } = new List<Decision>();

    [InverseProperty("IdResponseTeamNavigation")]
    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();

    [InverseProperty("IdResponseTeamNavigation")]
    public virtual ICollection<SystemUserResponseTeam> SystemUserResponseTeams { get; set; } = new List<SystemUserResponseTeam>();
}
