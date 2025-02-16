using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Xml;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;
using WebApp.Data.Configurations;

namespace WebApp.Data.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }
    //public DbSet<Order> Orders { get; set; }
    //public DbSet<OrderItem> OrderItems { get; set; }
    //public DbSet<Customer> Customers { get; set; }
    //public DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply SoftDelete Filtering

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;

            if (typeof(ISoftDeletable).IsAssignableFrom(entityClrType))
            {
                var parameter = Expression.Parameter(entityClrType, "e");
                var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                builder.Entity(entityClrType).HasQueryFilter(lambda);
            }
        }

        // Add Configuartion For Each Table

        builder.ApplyConfiguration(new ApplicationUserConfig());
        builder.ApplyConfiguration(new ProductConfig());
        builder.ApplyConfiguration(new CategoryConfig());
        builder.ApplyConfiguration(new SupplierConfig());
        //builder.ApplyConfiguration(new OrderCongig());
        //builder.ApplyConfiguration(new OrderItemCongig());
        //builder.ApplyConfiguration(new CustomerConfig());
        //builder.ApplyConfiguration(new AdminConfig());

        // Overriding Identity Tables Names

        builder.Entity<ApplicationUser>().ToTable("User");
        builder.Entity<IdentityRole>().ToTable("Role");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");

        // Database Seeding
        //DatabaseSeeder.Seed(builder);
    }
}