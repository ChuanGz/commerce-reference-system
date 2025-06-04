using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetAllOrdersQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetAllOrdersQuery, List<Order>>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<List<Order>> Handle(
        GetAllOrdersQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetAllAsync(cancellationToken);
    }
}
