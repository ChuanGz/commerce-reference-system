using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers {
    public class GetAllOrdersQueryHandler(IOrderRepository repo)
        : IRequestHandler<GetAllOrdersQuery, List<Order>> {
        public async Task<List<Order>> Handle(
            GetAllOrdersQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);

            return await repo.GetAllAsync(cancellationToken);
        }
    }
}
