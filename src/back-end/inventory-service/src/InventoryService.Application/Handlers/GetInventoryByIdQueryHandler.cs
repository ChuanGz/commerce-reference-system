using InventoryService.Application.Queries;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class GetInventoryByIdQueryHandler(IInventoryRepository repo)
    : IRequestHandler<GetInventoryByIdQuery, Inventory?>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<Inventory?> Handle(
        GetInventoryByIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetByIdAsync(request.Id, cancellationToken);
    }
}
