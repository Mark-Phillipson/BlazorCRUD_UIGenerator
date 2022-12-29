using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

public partial class MousePosition
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    public string Command { get; set; } = null!;

    public int MouseLeft { get; set; }

    public int MouseTop { get; set; }

    [StringLength(255)]
    public string? TabPageName { get; set; }

    [StringLength(255)]
    public string? ControlName { get; set; }

    [StringLength(255)]
    public string? Application { get; set; }
}
