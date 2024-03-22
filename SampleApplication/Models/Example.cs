using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Example")]
public class Example
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated { get; set; }

    [Required]
    [StringLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public required string CreatedBy { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; } = 0;

    [Required]
    public int Quantity { get; set; }

    [Required]
    public int CategoryId { get; set; }
}
