using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Core.DTOs;

public class ProductDTO
{
    [Required, MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required, ForeignKey("SupplierId")]
    public int SupplierId { get; set; }
}