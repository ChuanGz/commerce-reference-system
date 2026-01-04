using OrderService.Application.Commands;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers {
    public class CreateOrderCommandHandler(IOrderRepository repo)
        : IRequestHandler<CreateOrderCommand, Guid> {
        public async Task<Guid> Handle(
            CreateOrderCommand command,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(command);

            var order = new Order {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                ShippingAddress = command.ShippingAddress.Trim(),
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                OrderItems = command
                    .OrderItems.Select(item => new OrderItem {
                        Id = Guid.NewGuid(),
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    })
                    .ToList(),
            };

            order.RecalculateTotalAmount();

            await repo.AddAsync(order, cancellationToken);
            return order.Id;
        }
    }
}
