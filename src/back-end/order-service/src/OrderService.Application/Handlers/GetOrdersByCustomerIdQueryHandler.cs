using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetOrdersByCustomerIdQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersByCustomerIdQuery, List<Order>>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<List<Order>> Handle(
        GetOrdersByCustomerIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
    }
}
