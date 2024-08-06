
using System.ComponentModel.DataAnnotations;

namespace SampleApplication.DTOs
{
    public partial class CustomIntelliSenseDTO
    {
        [Key]
        public int Id { get; set; }        [Required]
        public int LanguageId { get; set; }        [Required]
        [StringLength(255)]
        public string DisplayValue { get; set; } ="";
        public string? SendKeysValue { get; set; }
        [StringLength(255)]
        public string? CommandType { get; set; }
        [Required]
        public int CategoryId { get; set; }        [StringLength(255)]
        public string? Remarks { get; set; }
        public string? Search { get; set; }
        public int? ComputerId { get; set; }
        [Required]
        [StringLength(30)]
        public string DeliveryType { get; set; } ="";
        [StringLength(60)]
        public string? Variable1 { get; set; }
        [StringLength(60)]
        public string? Variable2 { get; set; }
        [StringLength(60)]
        public string? Variable3 { get; set; }
    }   
}