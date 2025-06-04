using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class GetPaymentByIdQueryHandler(IPaymentRepository repo)
    : IRequestHandler<GetPaymentByIdQuery, Payment?>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<Payment?> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetByIdAsync(request.Id, cancellationToken);
    }
}
