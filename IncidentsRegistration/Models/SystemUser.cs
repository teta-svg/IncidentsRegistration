using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("System_user")]
[Index("LoginName", Name = "UQ__System_u__F6D56B57317E21CF", IsUnique = true)]
public partial class SystemUser
{
    [Key]
    [Column("ID_user")]
    public int IdUser { get; set; }

    [Column("login_name")]
    [StringLength(50)]
    public string LoginName { get; set; } = null!;

    [Column("user_password")]
    [StringLength(100)]
    public string UserPassword { get; set; } = null!;

    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<SystemUserResponseTeam> SystemUserResponseTeams { get; set; } = new List<SystemUserResponseTeam>();
}
