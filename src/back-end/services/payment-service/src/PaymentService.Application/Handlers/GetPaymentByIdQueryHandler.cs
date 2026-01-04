using PaymentService.Application.Queries;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers {
    public class GetPaymentByIdQueryHandler(IPaymentRepository repo)
        : IRequestHandler<GetPaymentByIdQuery, Payment?> {
        public async Task<Payment?> Handle(
            GetPaymentByIdQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}
