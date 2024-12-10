
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleApplication.DTOs
{
    public partial class LanguageDTO
    {
        [Key]
        public int Id { get; set; }        [Required(ErrorMessage = "⚠️ Language Name is required")]
        [StringLength(25)]
        public string LanguageName { get; set; } ="";
        [Required(ErrorMessage = "⚠️ Active is required")]
        public bool Active { get; set; }        [StringLength(40)]
        public string? Colour { get; set; }
    }   
}