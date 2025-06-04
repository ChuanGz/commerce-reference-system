using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class ReserveInventoryCommandHandler(IInventoryRepository repo)
    : IRequestHandler<ReserveInventoryCommand, Unit>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<Unit> Handle(
        ReserveInventoryCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        await _repo.ReserveQuantityAsync(request.ProductId, request.Quantity, cancellationToken);
        return Unit.Value;
    }
}
