using InventoryService.Application.Queries;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class GetAllInventoryQueryHandler(IInventoryRepository repo)
    : IRequestHandler<GetAllInventoryQuery, List<Inventory>>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<List<Inventory>> Handle(
        GetAllInventoryQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetAllAsync(cancellationToken);
    }
}
