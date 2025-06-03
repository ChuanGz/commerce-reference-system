using PaymentService.Application.Commands;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class ProcessPaymentCommandHandler(IPaymentRepository repo) : IRequestHandler<ProcessPaymentCommand, Unit>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<Unit> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payment = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (payment == null)
            throw new InvalidOperationException("Payment not found");

        if (payment.Status != "Pending")
            throw new InvalidOperationException("Payment is not in pending status");

        var isSuccessful = await SimulatePaymentProcessing(payment);
        
        payment.Status = isSuccessful ? "Completed" : "Failed";
        payment.TransactionId = isSuccessful ? $"TXN-{Guid.NewGuid():N}" : null;
        payment.ProcessedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(payment, cancellationToken);
        return Unit.Value;
    }

    private static async Task<bool> SimulatePaymentProcessing(Payment payment)
    {
        await Task.Delay(100);
        return payment.Amount <= 10000m;
    }
}
