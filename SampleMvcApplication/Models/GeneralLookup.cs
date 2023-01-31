using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class GeneralLookup
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Item_Value")]
    [StringLength(255)]
    public string ItemValue { get; set; } = null!;

    [StringLength(255)]
    public string Category { get; set; } = null!;

    public int? SortOrder { get; set; }

    [StringLength(255)]
    public string? DisplayValue { get; set; }
}
