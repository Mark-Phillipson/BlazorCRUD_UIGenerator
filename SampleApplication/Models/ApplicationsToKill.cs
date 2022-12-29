using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Table("ApplicationsToKill")]
[Index("CommandName", Name = "CommandName_UniqueKey", IsUnique = true)]
[Index("ProcessName", Name = "ProcessName_UniqueKey", IsUnique = true)]
public partial class ApplicationsToKill
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    [StringLength(50)]
    public string ProcessName { get; set; } = null!;

    [StringLength(255)]
    public string CommandName { get; set; } = null!;

    public bool Display { get; set; }
}
