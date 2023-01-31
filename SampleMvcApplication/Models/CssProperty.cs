using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class CssProperty
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string PropertyName { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }
}
