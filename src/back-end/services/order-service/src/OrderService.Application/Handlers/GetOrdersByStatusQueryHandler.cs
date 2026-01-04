using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers {
    public class GetOrdersByStatusQueryHandler(IOrderRepository repo)
        : IRequestHandler<GetOrdersByStatusQuery, List<Order>> {
        public async Task<List<Order>> Handle(
            GetOrdersByStatusQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);

            return await repo.GetByStatusAsync(query.Status, cancellationToken);
        }
    }
}
