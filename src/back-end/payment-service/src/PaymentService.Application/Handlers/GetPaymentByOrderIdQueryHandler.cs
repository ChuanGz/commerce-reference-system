using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers;

public class GetPaymentByOrderIdQueryHandler(IPaymentRepository repo) : IRequestHandler<GetPaymentByOrderIdQuery, Payment?>
{
    private readonly IPaymentRepository _repo = repo;

    public async Task<Payment?> Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken = default)
    {
        return await _repo.GetByOrderIdAsync(request.OrderId, cancellationToken);
    }
}
