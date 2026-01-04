namespace PaymentService.Application.Commands {
    public record CreatePaymentCommand(Guid OrderId, decimal Amount, string PaymentMethod)
        : IRequest<Guid>;
}
