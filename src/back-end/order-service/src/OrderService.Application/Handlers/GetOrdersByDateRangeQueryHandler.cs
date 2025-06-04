using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetOrdersByDateRangeQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersByDateRangeQuery, List<Order>>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<List<Order>> Handle(
        GetOrdersByDateRangeQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetOrdersByDateRangeAsync(
            request.StartDate,
            request.EndDate,
            cancellationToken
        );
    }
}
