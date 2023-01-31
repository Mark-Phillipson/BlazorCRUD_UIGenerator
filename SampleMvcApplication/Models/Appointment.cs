using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleMvcApplication.Models;

public partial class Appointment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int AppointmentType { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [StringLength(255)]
    public string? Caption { get; set; }

    public bool AllDay { get; set; }

    [StringLength(255)]
    public string? Location { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    public int Label { get; set; }

    public int Status { get; set; }

    [StringLength(255)]
    public string? Recurrence { get; set; }
}
