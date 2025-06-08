using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers
{
    public class ReserveInventoryCommandHandler(IInventoryRepository repo)
        : IRequestHandler<ReserveInventoryCommand, Unit>
    {
        public async Task<Unit> Handle(
            ReserveInventoryCommand request,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(request);

            await repo.ReserveQuantityAsync(request.ProductId, request.Quantity, cancellationToken);
            return Unit.Value;
        }
    }
}
