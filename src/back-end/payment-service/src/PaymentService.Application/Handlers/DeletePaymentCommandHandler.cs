using PaymentService.Application.Commands;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class DeletePaymentCommandHandler(IPaymentRepository repo) : IRequestHandler<DeletePaymentCommand, Unit>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
