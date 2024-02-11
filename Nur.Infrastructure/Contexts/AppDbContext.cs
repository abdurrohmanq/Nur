using Nur.Domain.Entities.Orders;
using Nur.Domain.Entities.Users;
using Nur.Domain.Entities.Payments;
using Nur.Domain.Entities.Products;
using Nur.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Nur.Domain.Entities.Suppliers;
using Nur.Domain.Entities.Attachments;
using Nur.Infrastructure.Persistence.EntityTypeConfiguration;

namespace Nur.Infrastructure.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        
        modelBuilder.ApplyConfiguration(new CartEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new SupplierEntityTypeConfiguration());
    }
}