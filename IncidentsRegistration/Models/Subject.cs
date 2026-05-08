using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Table("Subject")]
public partial class Subject
{
    [Key]
    [Column("ID_Subject")]
    public int IdSubject { get; set; }

    [Column("Last_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("First_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Patronymic { get; set; }

    [Column("Number_of_convictions")]
    public int? NumberOfConvictions { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Settlement { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Street { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string House { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string? Room { get; set; }

    [InverseProperty("IdSubjectNavigation")]
    public virtual ICollection<SubjectRole> SubjectRoles { get; set; } = new List<SubjectRole>();
}
