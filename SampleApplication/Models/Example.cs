using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

public partial class Example
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int NumberValue { get; set; }

    [StringLength(255)]
    public string Text { get; set; } = null!;

    public string LargeText { get; set; } = null!;

    public bool Boolean { get; set; }

    public DateTime? DateValue { get; set; }
}
