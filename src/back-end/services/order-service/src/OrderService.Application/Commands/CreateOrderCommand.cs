namespace OrderService.Application.Commands {
    public record CreateOrderCommand(
        Guid CustomerId,
        string ShippingAddress,
        List<OrderItemDto> OrderItems
    ) : IRequest<Guid>;

    public record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
}
