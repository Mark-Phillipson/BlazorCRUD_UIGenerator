
using System.ComponentModel.DataAnnotations;

namespace SampleApplication.DTOs
{
    public partial class CategoryDTO
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        [Required]
        public string CategoryName { get; set; } = default!;
        [StringLength(255)]
        public string? CategoryType { get; set; }
        [Required]
        public bool Sensitive { get; set; }
        [StringLength(40)]
        public string? Colour { get; set; }
    }
}