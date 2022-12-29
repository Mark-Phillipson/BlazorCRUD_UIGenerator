using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Table("Launcher")]
[Index("CategoryId", Name = "IX_CategoryID")]
[Index("ComputerId", Name = "IX_ComputerID")]
public partial class Launcher
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? CommandLine { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [Column("ComputerID")]
    public int? ComputerId { get; set; }

    [StringLength(30)]
    public string? Colour { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Launchers")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("ComputerId")]
    [InverseProperty("Launchers")]
    public virtual Computer? Computer { get; set; }

    [InverseProperty("Launcher")]
    public virtual ICollection<LauncherMultipleLauncherBridge> LauncherMultipleLauncherBridges { get; } = new List<LauncherMultipleLauncherBridge>();
}
