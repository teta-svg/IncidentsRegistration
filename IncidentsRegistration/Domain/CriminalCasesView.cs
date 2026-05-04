using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Models;

[Keyless]
public partial class CriminalCasesView
{
    [Column("Номер дела")]
    public int НомерДела { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Квалификация { get; set; } = null!;

    [Column("Дата возбуждения")]
    public DateOnly ДатаВозбуждения { get; set; }

    [Column("Тип происшествия")]
    [StringLength(50)]
    [Unicode(false)]
    public string ТипПроисшествия { get; set; } = null!;
}
