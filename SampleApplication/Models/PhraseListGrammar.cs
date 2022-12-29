using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

public partial class PhraseListGrammar
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string PhraseListGrammarValue { get; set; } = null!;
}
