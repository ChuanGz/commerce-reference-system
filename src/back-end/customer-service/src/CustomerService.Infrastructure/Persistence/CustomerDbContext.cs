using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var seedCustomers = new[]
        {
            new Customer
            {
                Id = new Guid("10000000-0000-0000-0000-000000000001"),
                UserId = new Guid("00000000-0000-0000-0000-000000000001"),
                FirstName = "John",
                LastName = "Doe",
                Phone = "1234567890",
                Address = "123 Main St, City, State",
                CreatedAt = DateTime.UtcNow,
            },
            new Customer
            {
                Id = new Guid("10000000-0000-0000-0000-000000000002"),
                UserId = new Guid("00000000-0000-0000-0000-000000000002"),
                FirstName = "Jane",
                LastName = "Smith",
                Phone = "0987654321",
                Address = "456 Oak Ave, City, State",
                CreatedAt = DateTime.UtcNow,
            },
            new Customer
            {
                Id = new Guid("10000000-0000-0000-0000-000000000003"),
                UserId = new Guid("00000000-0000-0000-0000-000000000003"),
                FirstName = "Bob",
                LastName = "Johnson",
                Phone = "5555555555",
                Address = "789 Pine Rd, City, State",
                CreatedAt = DateTime.UtcNow,
            },
            new Customer
            {
                Id = new Guid("10000000-0000-0000-0000-000000000004"),
                UserId = new Guid("00000000-0000-0000-0000-000000000004"),
                FirstName = "Alice",
                LastName = "Brown",
                Phone = "1111111111",
                Address = "321 Elm St, City, State",
                CreatedAt = DateTime.UtcNow,
            },
            new Customer
            {
                Id = new Guid("10000000-0000-0000-0000-000000000005"),
                UserId = new Guid("00000000-0000-0000-0000-000000000005"),
                FirstName = "Charlie",
                LastName = "Wilson",
                Phone = "2222222222",
                Address = "654 Maple Dr, City, State",
                CreatedAt = DateTime.UtcNow,
            },
        };

        modelBuilder.Entity<Customer>().HasData(seedCustomers);
    }
}
