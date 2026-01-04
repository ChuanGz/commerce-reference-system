using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers {
    public class GetOrderByIdQueryHandler(IOrderRepository repo)
        : IRequestHandler<GetOrderByIdQuery, Order?> {
        public async Task<Order?> Handle(
            GetOrderByIdQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);

            return await repo.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}
