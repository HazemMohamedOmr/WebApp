using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Core.Models;

namespace WebApp.Data.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}