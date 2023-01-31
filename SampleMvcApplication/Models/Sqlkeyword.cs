using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

[Table("SQLkeywords")]
public partial class Sqlkeyword
{
    [Key]
    [Column("column1")]
    [StringLength(50)]
    public string Column1 { get; set; } = null!;
}
