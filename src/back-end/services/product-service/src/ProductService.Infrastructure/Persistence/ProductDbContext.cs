using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Persistence {
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options) {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.SKU).IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasIndex(e => e.SKU).IsUnique();
                entity.HasIndex(e => e.Category);
            });

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder) {
            var products = new[]
            {
                new Product
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Wireless Bluetooth Headphones",
                    Description =
                        "High-quality wireless headphones with noise cancellation and 30-hour battery life",
                    Price = 199.99m,
                    Category = "Electronics",
                    SKU = "WBH-001",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                },
                new Product
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Ergonomic Office Chair",
                    Description =
                        "Comfortable office chair with lumbar support and adjustable height",
                    Price = 299.99m,
                    Category = "Furniture",
                    SKU = "EOC-002",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                },
                new Product
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Stainless Steel Water Bottle",
                    Description =
                        "Insulated water bottle that keeps drinks cold for 24 hours or hot for 12 hours",
                    Price = 29.99m,
                    Category = "Sports",
                    SKU = "SSWB-003",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                },
                new Product
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Organic Cotton T-Shirt",
                    Description = "Soft and comfortable t-shirt made from 100% organic cotton",
                    Price = 24.99m,
                    Category = "Clothing",
                    SKU = "OCT-004",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow,
                },
                new Product
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Smart Fitness Tracker",
                    Description = "Advanced fitness tracker with heart rate monitoring and GPS",
                    Price = 149.99m,
                    Category = "Electronics",
                    SKU = "SFT-005",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                },
            };

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
