using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Commands;
using OrderService.Application.Handlers;
using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using Xunit;

namespace OrderService.UnitTests;

public class OrderHandlersTests {
    [Fact]
    public async Task CreateOrderCommandHandler_PersistsOrder() {
        var dbContext = BuildDbContext();
        var repository = new OrderRepository(dbContext);
        var handler = new CreateOrderCommandHandler(repository);

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            "123 Test Street, City, Country",
            new List<OrderItemDto> { new(Guid.NewGuid(), 2, 10) }
        );

        var orderId = await handler.Handle(command, CancellationToken.None);

        var saved = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        Assert.NotNull(saved);
        Assert.Equal("Pending", saved!.Status);
        Assert.Equal(20, saved.TotalAmount);
    }

    [Fact]
    public async Task GetOrderByIdQueryHandler_ReturnsOrder() {
        var dbContext = BuildDbContext();
        var repository = new OrderRepository(dbContext);
        var handler = new GetOrderByIdQueryHandler(repository);

        var seeded = new Order {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            ShippingAddress = "Seed Address",
            Status = "Pending",
            OrderDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            OrderItems = [
                new OrderItem {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    Quantity = 1,
                    UnitPrice = 5,
                },
            ],
            TotalAmount = 5,
        };

        dbContext.Orders.Add(seeded);
        await dbContext.SaveChangesAsync();

        var result = await handler.Handle(
            new GetOrderByIdQuery(seeded.Id),
            CancellationToken.None
        );

        Assert.NotNull(result);
        Assert.Equal(seeded.Id, result!.Id);
    }

    private static OrderDbContext BuildDbContext() {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new OrderDbContext(options);
    }
}
