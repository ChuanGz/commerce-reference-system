using InventoryService.Application.Queries;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class GetInventoryByProductIdQueryHandler(IInventoryRepository repo) : IRequestHandler<GetInventoryByProductIdQuery, Inventory?>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<Inventory?> Handle(GetInventoryByProductIdQuery request, CancellationToken cancellationToken = default)
    {
        return await _repo.GetByProductIdAsync(request.ProductId, cancellationToken);
    }
}
