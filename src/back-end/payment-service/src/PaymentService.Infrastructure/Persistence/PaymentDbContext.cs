using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Constants;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Persistence;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.ProcessedAt).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.OrderId).IsUnique();
            entity.HasIndex(e => e.TransactionId).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.PaymentMethod);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var payments = new[]
        {
            new Payment
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                OrderId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Amount = 199.99m,
                PaymentMethod = "Credit Card",
                Status = PaymentStatus.Completed,
                TransactionId = "TXN-ABC123456789",
                ProcessedAt = DateTime.UtcNow.AddDays(-2),
                CreatedAt = DateTime.UtcNow.AddDays(-2),
            },
            new Payment
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                OrderId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Amount = 89.50m,
                PaymentMethod = "PayPal",
                Status = PaymentStatus.Completed,
                TransactionId = "TXN-DEF987654321",
                ProcessedAt = DateTime.UtcNow.AddDays(-1),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
            },
            new Payment
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                OrderId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Amount = 299.99m,
                PaymentMethod = "Debit Card",
                Status = PaymentStatus.Pending,
                TransactionId = null,
                ProcessedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
            },
            new Payment
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                OrderId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Amount = 45.00m,
                PaymentMethod = "Bank Transfer",
                Status = PaymentStatus.Failed,
                TransactionId = null,
                ProcessedAt = DateTime.UtcNow.AddHours(-6),
                CreatedAt = DateTime.UtcNow.AddHours(-6),
            },
        };

        modelBuilder.Entity<Payment>().HasData(payments);
    }
}
