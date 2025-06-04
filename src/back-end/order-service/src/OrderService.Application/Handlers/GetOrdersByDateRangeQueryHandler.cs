using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetOrdersByDateRangeQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersByDateRangeQuery, List<Order>>
{
    public async Task<List<Order>> Handle(
        GetOrdersByDateRangeQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);

        return await repo.GetOrdersByDateRangeAsync(
            query.StartDate,
            query.EndDate,
            cancellationToken
        );
    }
}
