using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.DTOs;

public class RegisterRequestDTO
{
    [Required] [StringLength(100)] public string FirstName { get; set; }

    [Required] [StringLength(100)] public string LastName { get; set; }

    [Required] [StringLength(128)] public string Email { get; set; }

    [Required] [StringLength(256)] public string Password { get; set; }

    [Required] [StringLength(100)] public string Role { get; set; }
}