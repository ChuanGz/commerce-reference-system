using InventoryService.Application.Queries;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers {
    public class GetInventoryByProductIdQueryHandler(IInventoryRepository repo)
        : IRequestHandler<GetInventoryByProductIdQuery, Inventory?> {
        public async Task<Inventory?> Handle(
            GetInventoryByProductIdQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetByProductIdAsync(query.ProductId, cancellationToken);
        }
    }
}
