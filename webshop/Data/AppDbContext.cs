using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using webshop.Models;

namespace webshop.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShippingProvider> ShippingProviders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ShippingProvider>().ToTable("ShippingProvider");
            modelBuilder.Entity<Customer>().ToTable("Customer");

            // Define primary keys
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerID);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderID);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductID);
            modelBuilder.Entity<ShippingProvider>().HasKey(sp => sp.ShippingProviderID);
            modelBuilder.Entity<OrderDetail>().HasKey(od => od.OrderDetailId);

            // Define relationships
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Customer)
            //    .WithMany() // No navigation property in Customer
            //    .HasForeignKey(o => o.CustomerID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingProvider)
                .WithMany(sp => sp.Orders)
                .HasForeignKey(o => o.ShippingProviderID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define precision for decimal properties
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
