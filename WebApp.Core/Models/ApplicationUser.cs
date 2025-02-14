using Microsoft.AspNetCore.Identity;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Models
{
    public class ApplicationUser : IdentityUser, IBaseEntity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Implement IBaseEntity properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}