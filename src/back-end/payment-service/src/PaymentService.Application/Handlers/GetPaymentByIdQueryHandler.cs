using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class GetPaymentByIdQueryHandler(IPaymentRepository repo)
    : IRequestHandler<GetPaymentByIdQuery, Payment?>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<Payment?> Handle(
        GetPaymentByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repo.GetByIdAsync(query.Id, cancellationToken);
    }
}
