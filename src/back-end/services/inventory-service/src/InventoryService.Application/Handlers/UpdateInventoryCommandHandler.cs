using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers
{
    public class UpdateInventoryCommandHandler(IInventoryRepository repo)
        : IRequestHandler<UpdateInventoryCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateInventoryCommand request,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(request);

            var inventory = await repo.GetByIdAsync(request.Id, cancellationToken);
            if (inventory == null)
                return Unit.Value;

            inventory.Quantity = request.Quantity;
            inventory.Location = request.Location.Trim();
            inventory.LastUpdated = DateTime.UtcNow;

            await repo.UpdateAsync(inventory, cancellationToken);
            return Unit.Value;
        }
    }
}
