
using System.ComponentModel.DataAnnotations;

namespace SampleApplication.DTOs
{
    public partial class GeneralLookupDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string ItemValue { get; set; } = "";
        [Required]
        [StringLength(255)]
        public string Category { get; set; } = "";
        public int? SortOrder { get; set; }
        [StringLength(255)]
        public string? DisplayValue { get; set; }
    }
}