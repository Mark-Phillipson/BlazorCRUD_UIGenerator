using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

[Table("MultipleLauncher")]
public partial class MultipleLauncher
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(70)]
    public string Description { get; set; } = null!;

    [InverseProperty("MultipleLauncher")]
    public virtual ICollection<LauncherMultipleLauncherBridge> LauncherMultipleLauncherBridges { get; } = new List<LauncherMultipleLauncherBridge>();
}
