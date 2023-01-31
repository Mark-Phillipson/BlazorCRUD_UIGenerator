using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

[Table("ValuesToInsert")]
public partial class ValuesToInsert
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    public string ValueToInsert { get; set; } = null!;

    [StringLength(255)]
    public string Lookup { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }
}
