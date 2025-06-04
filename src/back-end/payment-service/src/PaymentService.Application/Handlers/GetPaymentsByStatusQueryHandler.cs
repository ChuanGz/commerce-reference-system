using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class GetPaymentsByStatusQueryHandler(IPaymentRepository repo)
    : IRequestHandler<GetPaymentsByStatusQuery, List<Payment>>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<List<Payment>> Handle(
        GetPaymentsByStatusQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetByStatusAsync(request.Status, cancellationToken);
    }
}
