using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Table("WindowsSpeechVoiceCommand")]
public partial class WindowsSpeechVoiceCommand
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string SpokenCommand { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? ApplicationName { get; set; }

    [Required]
    public bool? AutoCreated { get; set; }

    [InverseProperty("WindowsSpeechVoiceCommand")]
    public virtual ICollection<CustomWindowsSpeechCommand> CustomWindowsSpeechCommands { get; } = new List<CustomWindowsSpeechCommand>();
}
