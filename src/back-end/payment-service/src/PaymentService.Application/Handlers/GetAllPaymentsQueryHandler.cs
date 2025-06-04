using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class GetAllPaymentsQueryHandler(IPaymentRepository repo)
    : IRequestHandler<GetAllPaymentsQuery, List<Payment>>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<List<Payment>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetAllAsync(cancellationToken);
    }
}
