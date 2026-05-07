using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Subject_role")]
public partial class SubjectRole
{
    [Key]
    [Column("ID_Subject_role")]
    public int IdSubjectRole { get; set; }

    [Column("ID_Incident")]
    public int IdIncident { get; set; }

    [Column("ID_Subject")]
    public int IdSubject { get; set; }

    [Column("Date_of_involvement")]
    public DateOnly DateOfInvolvement { get; set; }

    [Column("Role_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    [ForeignKey("IdIncident")]
    [InverseProperty("SubjectRoles")]
    public virtual Incident IdIncidentNavigation { get; set; } = null!;

    [ForeignKey("IdSubject")]
    [InverseProperty("SubjectRoles")]
    public virtual Subject IdSubjectNavigation { get; set; } = null!;
}
