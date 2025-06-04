using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class UpdateInventoryCommandHandler(IInventoryRepository repo)
    : IRequestHandler<UpdateInventoryCommand, Unit>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<Unit> Handle(
        UpdateInventoryCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var inventory = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (inventory == null)
            return Unit.Value;

        inventory.Quantity = request.Quantity;
        inventory.Location = request.Location.Trim();
        inventory.LastUpdated = DateTime.UtcNow;

        await _repo.UpdateAsync(inventory, cancellationToken);
        return Unit.Value;
    }
}
