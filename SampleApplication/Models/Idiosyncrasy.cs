using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

public partial class Idiosyncrasy
{
    [Key]
    public int Id { get; set; }

    [StringLength(60)]
    public string? FindString { get; set; }

    [StringLength(60)]
    public string? ReplaceWith { get; set; }
}
