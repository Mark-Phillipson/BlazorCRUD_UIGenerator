using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Table("LauncherMultipleLauncherBridge")]
public partial class LauncherMultipleLauncherBridge
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("LauncherID")]
    public int LauncherId { get; set; }

    [Column("MultipleLauncherID")]
    public int MultipleLauncherId { get; set; }

    [ForeignKey("LauncherId")]
    [InverseProperty("LauncherMultipleLauncherBridges")]
    public virtual Launcher Launcher { get; set; } = null!;

    [ForeignKey("MultipleLauncherId")]
    [InverseProperty("LauncherMultipleLauncherBridges")]
    public virtual MultipleLauncher MultipleLauncher { get; set; } = null!;
}
