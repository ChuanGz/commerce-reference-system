using PaymentService.Application.Commands;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class UpdatePaymentStatusCommandHandler(IPaymentRepository repo)
    : IRequestHandler<UpdatePaymentStatusCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdatePaymentStatusCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var payment = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (payment == null)
            return Unit.Value;

        payment.Status = request.Status.Trim();
        payment.TransactionId = request.TransactionId?.Trim();
        payment.ProcessedAt = DateTime.UtcNow;

        await repo.UpdateAsync(payment, cancellationToken);
        return Unit.Value;
    }
}
