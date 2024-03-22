
using System.ComponentModel.DataAnnotations;

namespace SampleApplication.DTOs
{
    public partial class ExampleDTO
    {
        [Key]
        public int Id { get; set; }        [Required]
        [StringLength(50)]
        public string Name { get; set; } ="";
        [Required]
        public string Description { get; set; } ="";
        [Required]
        public DateTime DateCreated { get; set; }        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; } ="";
        [Required]
        public bool IsActive { get; set; }        [Required]
        public decimal Price { get; set; }        [Required]
        public int Quantity { get; set; }        [Required]
        public int CategoryId { get; set; }    }   
}