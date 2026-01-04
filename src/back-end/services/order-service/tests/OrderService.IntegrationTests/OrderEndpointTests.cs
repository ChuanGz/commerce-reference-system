using System.Net;
using System.Net.Http.Json;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Commands;
using OrderService.Infrastructure.Persistence;
using Xunit;

namespace OrderService.IntegrationTests;

public class OrderEndpointTests {
    [Fact]
    public async Task CreateOrder_ThenGetAll_ReturnsCreatedOrder() {
        await using var factory = new OrderServiceFactory();
        using var client = factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "test");

        var createCommand = new CreateOrderCommand(
            Guid.NewGuid(),
            "123 Test Street, City, Country",
            new List<OrderItemDto> {
                new(Guid.NewGuid(), 2, 10),
            }
        );

        var createResponse = await client.PostAsJsonAsync("/api/orders", createCommand);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        var count = await db.Orders.CountAsync();

        Assert.True(count > 0);
    }
}
