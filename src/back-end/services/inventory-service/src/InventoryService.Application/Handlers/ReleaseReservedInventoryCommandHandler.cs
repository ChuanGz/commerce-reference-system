using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers
{
    public class ReleaseReservedInventoryCommandHandler(IInventoryRepository repo)
        : IRequestHandler<ReleaseReservedInventoryCommand, Unit>
    {
        public async Task<Unit> Handle(
            ReleaseReservedInventoryCommand request,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(request);

            await repo.ReleaseReservedQuantityAsync(
                request.ProductId,
                request.Quantity,
                cancellationToken
            );
            return Unit.Value;
        }
    }
}
