using PaymentService.Application.Commands;
using PaymentService.Domain.Constants;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class CreatePaymentCommandHandler(IPaymentRepository repo)
    : IRequestHandler<CreatePaymentCommand, Guid>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<Guid> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod.Trim(),
            Status = PaymentStatus.Pending,
            TransactionId = null,
            ProcessedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };

        await _repo.AddAsync(payment, cancellationToken);
        return payment.Id;
    }
}
