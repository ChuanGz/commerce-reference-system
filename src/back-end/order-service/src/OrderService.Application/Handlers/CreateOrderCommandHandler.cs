using OrderService.Application.Commands;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class CreateOrderCommandHandler(IOrderRepository repo)
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            ShippingAddress = request.ShippingAddress.Trim(),
            Status = "Pending",
            OrderDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            OrderItems = request
                .OrderItems.Select(item => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                })
                .ToList(),
        };

        order.TotalAmount = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);

        await _repo.AddAsync(order, cancellationToken);
        return order.Id;
    }
}
