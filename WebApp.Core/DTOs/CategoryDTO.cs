using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.DTOs;

public class CategoryDTO
{
    [Required, MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}