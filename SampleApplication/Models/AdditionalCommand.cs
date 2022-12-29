using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Index("CustomIntelliSenseId", Name = "IX_AdditionalCommands")]
public partial class AdditionalCommand
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CustomIntelliSenseID")]
    public int CustomIntelliSenseId { get; set; }

    [Column(TypeName = "decimal(1, 1)")]
    public decimal WaitBefore { get; set; }

    [Column("SendKeys_Value")]
    public string SendKeysValue { get; set; } = null!;

    [StringLength(255)]
    public string? Remarks { get; set; }

    [StringLength(30)]
    public string DeliveryType { get; set; } = null!;

    [ForeignKey("CustomIntelliSenseId")]
    [InverseProperty("AdditionalCommands")]
    public virtual CustomIntelliSense CustomIntelliSense { get; set; } = null!;
}
