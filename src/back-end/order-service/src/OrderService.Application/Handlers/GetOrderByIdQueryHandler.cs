using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetOrderByIdQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetOrderByIdQuery, Order?>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<Order?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetByIdAsync(request.Id, cancellationToken);
    }
}
