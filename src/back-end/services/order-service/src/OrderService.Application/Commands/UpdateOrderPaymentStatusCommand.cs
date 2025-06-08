namespace OrderService.Application.Commands;

public record UpdateOrderPaymentStatusCommand(Guid OrderId, string PaymentStatus) : IRequest<bool>;
