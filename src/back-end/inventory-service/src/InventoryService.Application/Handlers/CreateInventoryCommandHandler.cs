using InventoryService.Application.Commands;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class CreateInventoryCommandHandler(IInventoryRepository repo)
    : IRequestHandler<CreateInventoryCommand, Guid>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<Guid> Handle(
        CreateInventoryCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var inventory = new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            ReservedQuantity = 0,
            Location = request.Location.Trim(),
            LastUpdated = DateTime.UtcNow,
        };

        await _repo.AddAsync(inventory, cancellationToken);
        return inventory.Id;
    }
}
