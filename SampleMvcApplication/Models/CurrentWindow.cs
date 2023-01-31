using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

[Table("CurrentWindow")]
public partial class CurrentWindow
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int Handle { get; set; }
}
