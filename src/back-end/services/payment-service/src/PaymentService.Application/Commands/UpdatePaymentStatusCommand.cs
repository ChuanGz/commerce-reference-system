namespace PaymentService.Application.Commands {
    public record UpdatePaymentStatusCommand(Guid Id, string Status, string? TransactionId)
        : IRequest<Unit>;
}
