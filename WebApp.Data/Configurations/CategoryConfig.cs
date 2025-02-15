using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Core.Models;

namespace WebApp.Data.Configurations;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
        entity.Property(p => p.Description).IsRequired(false).HasMaxLength(500);

        // Relationships
        entity.HasMany(p => p.Products)
            .WithOne(c => c.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}