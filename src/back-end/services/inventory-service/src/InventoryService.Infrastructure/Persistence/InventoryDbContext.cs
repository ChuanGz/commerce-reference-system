using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence {
    public class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : DbContext(options) {
        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inventory>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ReservedQuantity).IsRequired();
                entity.Property(e => e.Location).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastUpdated).IsRequired();

                entity.HasIndex(e => e.ProductId).IsUnique();
            });

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder) {
            var baseDate = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var inventories = new[]
            {
                new Inventory
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Quantity = 100,
                    ReservedQuantity = 0,
                    Location = "Warehouse A - Shelf 1",
                    LastUpdated = baseDate,
                },
                new Inventory
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Quantity = 50,
                    ReservedQuantity = 5,
                    Location = "Warehouse B - Shelf 2",
                    LastUpdated = baseDate,
                },
                new Inventory
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    ProductId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Quantity = 25,
                    ReservedQuantity = 0,
                    Location = "Warehouse A - Shelf 3",
                    LastUpdated = baseDate,
                },
            };

            modelBuilder.Entity<Inventory>().HasData(inventories);
        }
    }
}
