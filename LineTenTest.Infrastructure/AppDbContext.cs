using LineTenTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LineTenTest.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure cascade delete for Product -> Orders
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}