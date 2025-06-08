using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class GetAllPaymentsQueryHandler(IPaymentRepository repo)
    : IRequestHandler<GetAllPaymentsQuery, List<Payment>>
{
    public async Task<List<Payment>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await repo.GetAllAsync(cancellationToken);
    }
}
