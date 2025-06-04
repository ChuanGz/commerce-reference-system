using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetOrdersByCustomerIdQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersByCustomerIdQuery, List<Order>>
{
    public async Task<List<Order>> Handle(
        GetOrdersByCustomerIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        return await repo.GetByCustomerIdAsync(query.CustomerId, cancellationToken);
    }
}
