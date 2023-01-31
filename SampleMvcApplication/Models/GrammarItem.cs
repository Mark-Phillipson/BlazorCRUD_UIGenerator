using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

[Index("GrammarNameId", Name = "IX_GrammarItems_GrammarNameId")]
public partial class GrammarItem
{
    [Key]
    public int Id { get; set; }

    public int GrammarNameId { get; set; }

    [StringLength(60)]
    public string Value { get; set; } = null!;

    [ForeignKey("GrammarNameId")]
    [InverseProperty("GrammarItems")]
    public virtual GrammarName GrammarName { get; set; } = null!;
}
