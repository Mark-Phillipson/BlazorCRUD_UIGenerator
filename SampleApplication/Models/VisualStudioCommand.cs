using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Index("Caption", Name = "IX_VisualStudioCommands")]
public partial class VisualStudioCommand
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    public string Caption { get; set; } = null!;

    [StringLength(255)]
    public string Command { get; set; } = null!;
}
