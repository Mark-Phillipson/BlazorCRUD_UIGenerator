using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Table("SavedMousePosition")]
public partial class SavedMousePosition
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    public string NamedLocation { get; set; } = null!;

    public int X { get; set; }

    public int Y { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created { get; set; }
}
