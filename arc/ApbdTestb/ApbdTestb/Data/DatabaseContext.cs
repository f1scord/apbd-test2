using ApbdTestb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApbdTestb.Data;

public class DatabaseContext : DbContext
{
    public DbSet<User>      Users      { get; set; }
    public DbSet<Product>   Products   { get; set; }
    public DbSet<Order>     Orders     { get; set; }
    public DbSet<Payment>   Payments   { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new List<User>
        {
            new() { UserId = 1, Username = "john_doe", Email = "john@example.com", PasswordHash = "hash1", CreatedAt = DateTime.Today },
            new() { UserId = 2, Username = "jane_doe", Email = "jane@example.com", PasswordHash = "hash2", CreatedAt = DateTime.Today },
        });

        modelBuilder.Entity<Product>().HasData(new List<Product>
        {
            new() { ProductId = 1, Name = "Laptop",  Description = "Gaming laptop",   Price = 1200,  StockQuantity = 15  },
            new() { ProductId = 2, Name = "Mouse",   Description = "Wireless mouse",  Price = 25.5m, StockQuantity = 100 },
            new() { ProductId = 3, Name = "Monitor", Description = "4K monitor",      Price = 400,   StockQuantity = 30  },
        });

        modelBuilder.Entity<Order>().HasData(new List<Order>
        {
            new() { OrderId = 1, OrderDate = DateTime.Parse("2025-04-01"), Status = "Completed",  TotalAmount = 1250, Users_UserId = 1 },
            new() { OrderId = 2, OrderDate = DateTime.Parse("2025-04-05"), Status = "Pending",    TotalAmount = 400,  Users_UserId = 2 },
        });

        modelBuilder.Entity<Payment>().HasData(new List<Payment>
        {
            new() { PaymentId = 1, PaymentMethod = "Credit Card", Amount = 1250, PaymentStatus = "Paid", Orders_OrderId = 1 },
        });

        modelBuilder.Entity<OrderItem>().HasData(new List<OrderItem>
        {
            new() { OrderId = 1, ProductId = 1, Quantity = 1,  Price = 1200 },
            new() { OrderId = 1, ProductId = 2, Quantity = 2,  Price = 25   },
            new() { OrderId = 2, ProductId = 3, Quantity = 1,  Price = 400  },
        });
    }
}
