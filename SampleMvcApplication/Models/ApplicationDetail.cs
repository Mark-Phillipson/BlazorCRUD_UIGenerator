using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class ApplicationDetail
{
    [Key]
    public int Id { get; set; }

    [StringLength(60)]
    public string ProcessName { get; set; } = null!;

    [StringLength(255)]
    public string ApplicationTitle { get; set; } = null!;
}
