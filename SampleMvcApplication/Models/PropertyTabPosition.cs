using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class PropertyTabPosition
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(60)]
    public string ObjectName { get; set; } = null!;

    [StringLength(60)]
    public string PropertyName { get; set; } = null!;

    public int NumberOfTabs { get; set; }
}
