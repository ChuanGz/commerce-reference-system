using PaymentService.Application.Commands;
using PaymentService.Domain.Constants;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class ProcessPaymentCommandHandler(IPaymentRepository repo)
    : IRequestHandler<ProcessPaymentCommand, Unit>
{
    public async Task<Unit> Handle(
        ProcessPaymentCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var payment = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (payment == null)
            throw new InvalidOperationException(ErrorMessages.PaymentNotFound);

        if (payment.Status != PaymentStatus.Pending)
            throw new InvalidOperationException(ErrorMessages.PaymentNotPending);

        var isSuccessful = await SimulatePaymentProcessing(payment);

        payment.Status = isSuccessful ? PaymentStatus.Completed : PaymentStatus.Failed;
        payment.TransactionId = isSuccessful ? $"TXN-{Guid.NewGuid():N}" : null;
        payment.ProcessedAt = DateTime.UtcNow;

        await repo.UpdateAsync(payment, cancellationToken);
        return Unit.Value;
    }

    private static async Task<bool> SimulatePaymentProcessing(Payment payment)
    {
        await Task.Delay(100);
        return payment.Amount <= 10000m;
    }
}
