using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Core.Models;

namespace WebApp.Data.Configurations;

public class SupplierConfig : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> entity)
    {
        entity.Property(p => p.CompanyName).IsRequired().HasMaxLength(100);
        entity.Property(p => p.ContactInfo).HasMaxLength(500);

        // Relationships
        entity.HasMany(p => p.Products)
            .WithOne(c => c.Supplier)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}