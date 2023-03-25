
using System.ComponentModel.DataAnnotations;

namespace ARM_BlazorServer.DTOs
{
    public partial class AUserDTO
    {
        [Key]
        public int UserId { get; set; }        [StringLength(255)]
        public string? AuthUser { get; set; }
        [StringLength(255)]
        public string? BArea { get; set; }
        [Required]
        public DateTime Requested { get; set; }        [Required]
        public bool Granted { get; set; }        [Required]
        public bool InternalUse { get; set; }        [Required]
        public bool IssueEditor { get; set; }        [Required]
        public bool SlotEditor { get; set; }        [Required]
        public bool SlotViewer { get; set; }        [Required]
        public bool Survey { get; set; }        [Required]
        public bool AppEditor { get; set; }        [Required]
        public bool BookingEditor { get; set; }        [Required]
        public bool UserAdmin { get; set; }        public string? Note { get; set; }
        [Required]
        public bool ResourceEditor { get; set; }        [Required]
        public bool SysAdmin { get; set; }        [Required]
        public bool NotificationEditor { get; set; }        [StringLength(255)]
        public string? Email { get; set; }
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? OU { get; set; }
        public DateTime? LastAccess { get; set; }
        public bool Actor { get; set; }
        public int? RoleId { get; set; }
        public int? MaxRoleId { get; set; }
        public DateTime? LastAccessDT { get; set; }
        public bool SPOC { get; set; }
        [StringLength(5)]
        public string? LanguageCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool TestMe { get; set; }
    }   
}