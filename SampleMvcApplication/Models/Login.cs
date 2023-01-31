using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class Login
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(30)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string? Password { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    public DateTimeOffset? Created { get; set; }
}
