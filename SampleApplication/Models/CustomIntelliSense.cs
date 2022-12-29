using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Table("CustomIntelliSense")]
[Index("CategoryId", Name = "IX_CategoryID")]
[Index("ComputerId", Name = "IX_ComputerID")]
[Index("LanguageId", Name = "IX_LanguageID")]
public partial class CustomIntelliSense
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("LanguageID")]
    public int LanguageId { get; set; }

    [Column("Display_Value")]
    [StringLength(255)]
    public string DisplayValue { get; set; } = null!;

    [Column("SendKeys_Value")]
    public string? SendKeysValue { get; set; }

    [Column("Command_Type")]
    [StringLength(255)]
    public string? CommandType { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }

    public string? Search { get; set; }

    [Column("ComputerID")]
    public int? ComputerId { get; set; }

    [StringLength(30)]
    public string DeliveryType { get; set; } = null!;

    [InverseProperty("CustomIntelliSense")]
    public virtual ICollection<AdditionalCommand> AdditionalCommands { get; } = new List<AdditionalCommand>();

    [ForeignKey("CategoryId")]
    [InverseProperty("CustomIntelliSenses")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("ComputerId")]
    [InverseProperty("CustomIntelliSenses")]
    public virtual Computer? Computer { get; set; }

    [ForeignKey("LanguageId")]
    [InverseProperty("CustomIntelliSenses")]
    public virtual Language Language { get; set; } = null!;
}
