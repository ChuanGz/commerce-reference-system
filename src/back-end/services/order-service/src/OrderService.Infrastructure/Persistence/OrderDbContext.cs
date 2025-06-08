using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.OrderDate).IsRequired();
            entity.Property(e => e.ShippingAddress).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.OrderDate);

            entity
                .HasMany(e => e.OrderItems)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.ProductId).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");

            entity.HasIndex(e => e.OrderId);
            entity.HasIndex(e => e.ProductId);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var orders = new[]
        {
            new Order
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                TotalAmount = 299.98m,
                Status = "Delivered",
                OrderDate = DateTime.UtcNow.AddDays(-5),
                ShippingAddress = "123 Main St, New York, NY 10001",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
            },
            new Order
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CustomerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                TotalAmount = 149.99m,
                Status = "Shipped",
                OrderDate = DateTime.UtcNow.AddDays(-2),
                ShippingAddress = "456 Oak Ave, Los Angeles, CA 90210",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
            },
            new Order
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                CustomerId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                TotalAmount = 89.99m,
                Status = "Processing",
                OrderDate = DateTime.UtcNow.AddDays(-1),
                ShippingAddress = "789 Pine Rd, Chicago, IL 60601",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
            },
        };

        var orderItems = new[]
        {
            new OrderItem
            {
                Id = Guid.Parse("aa111111-1111-1111-1111-111111111111"),
                OrderId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ProductId = Guid.Parse("bb222222-2222-2222-2222-222222222222"),
                Quantity = 1,
                UnitPrice = 199.99m,
            },
            new OrderItem
            {
                Id = Guid.Parse("aa222222-2222-2222-2222-222222222222"),
                OrderId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ProductId = Guid.Parse("bb333333-3333-3333-3333-333333333333"),
                Quantity = 2,
                UnitPrice = 49.99m,
            },
            new OrderItem
            {
                Id = Guid.Parse("aa333333-3333-3333-3333-333333333333"),
                OrderId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ProductId = Guid.Parse("bb444444-4444-4444-4444-444444444444"),
                Quantity = 1,
                UnitPrice = 149.99m,
            },
            new OrderItem
            {
                Id = Guid.Parse("aa444444-4444-4444-4444-444444444444"),
                OrderId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ProductId = Guid.Parse("bb555555-5555-5555-5555-555555555555"),
                Quantity = 3,
                UnitPrice = 29.99m,
            },
        };

        modelBuilder.Entity<Order>().HasData(orders);
        modelBuilder.Entity<OrderItem>().HasData(orderItems);
    }
}
