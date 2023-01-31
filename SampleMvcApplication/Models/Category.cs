using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

[Index("CategoryName", "CategoryType", Name = "IX_Categories", IsUnique = true)]
public partial class Category
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Category")]
    [StringLength(30)]
    public string? CategoryName { get; set; }

    [Column("Category_Type")]
    [StringLength(255)]
    public string? CategoryType { get; set; }

    public bool Sensitive { get; set; }

    [StringLength(40)]
    public string? Colour { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<CustomIntelliSense> CustomIntelliSenses { get; } = new List<CustomIntelliSense>();

    [InverseProperty("Category")]
    public virtual ICollection<Launcher> Launchers { get; } = new List<Launcher>();
}
