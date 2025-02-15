using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.DTOs;

public class SupplierDTO
{
    [Required, MaxLength(100)]
    public string CompanyName { get; set; }

    [MaxLength(500)]
    public string? ContactInfo { get; set; }
}