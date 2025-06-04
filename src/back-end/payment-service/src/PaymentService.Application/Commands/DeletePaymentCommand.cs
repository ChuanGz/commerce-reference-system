namespace PaymentService.Application.Commands;

public record DeletePaymentCommand(Guid Id) : IRequest<Unit>;
