using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetOrdersByStatusQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersByStatusQuery, List<Order>>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<List<Order>> Handle(
        GetOrdersByStatusQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetByStatusAsync(request.Status, cancellationToken);
    }
}
