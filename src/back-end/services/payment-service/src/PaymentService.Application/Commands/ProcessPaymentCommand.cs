namespace PaymentService.Application.Commands
{
    public record ProcessPaymentCommand(Guid Id) : IRequest<Unit>;
}
