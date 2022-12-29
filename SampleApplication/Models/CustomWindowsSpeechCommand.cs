using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

[Index("WindowsSpeechVoiceCommandId", Name = "IX_CustomWindowsSpeechCommands_WindowsSpeechVoiceCommandId")]
public partial class CustomWindowsSpeechCommand
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? TextToEnter { get; set; }

    public int? KeyDownValue { get; set; }

    public int? ModifierKey { get; set; }

    public int? KeyPressValue { get; set; }

    [StringLength(100)]
    public string? MouseCommand { get; set; }

    [StringLength(255)]
    public string? ProcessStart { get; set; }

    [StringLength(255)]
    public string? CommandLineArguments { get; set; }

    public int WindowsSpeechVoiceCommandId { get; set; }

    [Required]
    public bool? AlternateKey { get; set; }

    [Required]
    public bool? ControlKey { get; set; }

    [Required]
    public bool? ShiftKey { get; set; }

    public int WaitTime { get; set; }

    public double AbsoluteX { get; set; }

    public double AbsoluteY { get; set; }

    public int MouseMoveX { get; set; }

    public int MouseMoveY { get; set; }

    public int ScrollAmount { get; set; }

    [Required]
    public bool? WindowsKey { get; set; }

    public int? KeyUpValue { get; set; }

    [StringLength(40)]
    public string? SendKeysValue { get; set; }

    [StringLength(55)]
    public string HowToFormatDictation { get; set; } = null!;

    [StringLength(255)]
    public string? MethodToCall { get; set; }

    [ForeignKey("WindowsSpeechVoiceCommandId")]
    [InverseProperty("CustomWindowsSpeechCommands")]
    public virtual WindowsSpeechVoiceCommand WindowsSpeechVoiceCommand { get; set; } = null!;
}
