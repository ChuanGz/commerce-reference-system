using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers {
    public class GetPaymentByOrderIdQueryHandler(IPaymentRepository repo)
        : IRequestHandler<GetPaymentByOrderIdQuery, Payment?> {
        public async Task<Payment?> Handle(
            GetPaymentByOrderIdQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetByOrderIdAsync(query.OrderId, cancellationToken);
        }
    }
}
