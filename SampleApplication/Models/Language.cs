using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Index("Language1", Name = "IX_Languages", IsUnique = true)]
public partial class Language
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Language")]
    [StringLength(25)]
    public string Language1 { get; set; } = null!;

    public bool Active { get; set; }

    [StringLength(40)]
    public string? Colour { get; set; }

    [InverseProperty("Language")]
    public virtual ICollection<CustomIntelliSense> CustomIntelliSenses { get; } = new List<CustomIntelliSense>();
}
