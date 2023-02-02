
using System.ComponentModel.DataAnnotations;

namespace SampleApplication.DTOs
{
    public partial class ExampleDTO
    {
        [Key]
        public int Id { get; set; }        [Required]
        public int NumberValue { get; set; }        [Required]
        [StringLength(255)]
        public string Text { get; set; } ="";
        [Required]
        public string LargeText { get; set; } ="";
        [Required]
        public bool Boolean { get; set; }        public DateTime? DateValue { get; set; }
    }   
}