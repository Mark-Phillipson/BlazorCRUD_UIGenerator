
using System.ComponentModel.DataAnnotations;

namespace SampleApplication.DTOs
{
    public partial class CustomerDTO
    {
        [Key]
        public int id { get; set; }        [Required]
        [StringLength(255)]
        public string CustomerName { get; set; } ="";
        [StringLength(255)]
        public string? ContactName { get; set; }
        [StringLength(255)]
        public string? Address { get; set; }
        [StringLength(255)]
        public string? City { get; set; }
        [StringLength(255)]
        public string? PostalCode { get; set; }
        [StringLength(255)]
        public string? Email { get; set; }
        [StringLength(255)]
        public string? Website { get; set; }
        [StringLength(255)]
        public string? Country { get; set; }
        public DateTime? Created { get; set; }
        public bool Active { get; set; }
    }   
}