using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class ReleaseReservedInventoryCommandHandler(IInventoryRepository repo)
    : IRequestHandler<ReleaseReservedInventoryCommand, Unit>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<Unit> Handle(
        ReleaseReservedInventoryCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        await _repo.ReleaseReservedQuantityAsync(
            request.ProductId,
            request.Quantity,
            cancellationToken
        );
        return Unit.Value;
    }
}
