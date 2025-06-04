using InventoryService.Application.Queries;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class GetInventoryByIdQueryHandler(IInventoryRepository repo)
    : IRequestHandler<GetInventoryByIdQuery, Inventory?>
{
    public async Task<Inventory?> Handle(
        GetInventoryByIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        return await repo.GetByIdAsync(request.Id, cancellationToken);
    }
}
