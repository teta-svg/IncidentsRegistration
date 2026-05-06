using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentsRegistration.Models;

public partial class SystemUser
{
    public int IdUser { get; set; }

    public string LoginName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string UserRole { get; set; } = null!;
    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<SystemUserResponseTeam> SystemUserResponseTeams { get; set; } = new List<SystemUserResponseTeam>();

}
