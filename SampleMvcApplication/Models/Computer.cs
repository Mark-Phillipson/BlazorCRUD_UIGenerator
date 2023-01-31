using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class Computer
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(20)]
    public string ComputerName { get; set; } = null!;

    [InverseProperty("Computer")]
    public virtual ICollection<CustomIntelliSense> CustomIntelliSenses { get; } = new List<CustomIntelliSense>();

    [InverseProperty("Computer")]
    public virtual ICollection<Launcher> Launchers { get; } = new List<Launcher>();
}
