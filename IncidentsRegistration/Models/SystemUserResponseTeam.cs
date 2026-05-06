using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentsRegistration.Models;

[Table("System_user_Response_team")]
public partial class SystemUserResponseTeam
{
    [Key]
    [Column("ID_System_user_Response_team")]
    public int IdSystemUserResponseTeam { get; set; }

    [Column("ID_Response_team")]
    public int IdResponseTeam { get; set; }

    [Column("ID_user")]
    public int IdUser { get; set; }

    [ForeignKey("IdResponseTeam")]
    [InverseProperty("SystemUserResponseTeams")]
    public virtual ResponseTeam IdResponseTeamNavigation { get; set; } = null!;

    [ForeignKey("IdUser")]
    [InverseProperty("SystemUserResponseTeams")]
    public virtual SystemUser IdUserNavigation { get; set; } = null!;
}
