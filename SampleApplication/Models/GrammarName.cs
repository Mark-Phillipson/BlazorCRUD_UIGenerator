using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Index("NameOfGrammar", Name = "IX_GrammarNames_NameOfGrammar", IsUnique = true)]
public partial class GrammarName
{
    [Key]
    public int Id { get; set; }

    [StringLength(40)]
    public string NameOfGrammar { get; set; } = null!;

    [InverseProperty("GrammarName")]
    public virtual ICollection<GrammarItem> GrammarItems { get; } = new List<GrammarItem>();
}
