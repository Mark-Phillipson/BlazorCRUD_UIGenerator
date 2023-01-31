using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class Todo
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string Description { get; set; } = null!;

    public bool Completed { get; set; }

    [StringLength(255)]
    public string? Project { get; set; }

    public bool Archived { get; set; }

    public DateTime Created { get; set; }

    public int SortPriority { get; set; }
}
