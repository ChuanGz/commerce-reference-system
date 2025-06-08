using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers
{
    public class GetPaymentsByStatusQueryHandler(IPaymentRepository repo)
        : IRequestHandler<GetPaymentsByStatusQuery, List<Payment>>
    {
        public async Task<List<Payment>> Handle(
            GetPaymentsByStatusQuery query,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetByStatusAsync(query.Status, cancellationToken);
        }
    }
}
