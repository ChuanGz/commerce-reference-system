using System.Linq.Expressions;
using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Repositories;

public interface IInventoryRepository
{
    Task<List<Inventory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Inventory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Inventory?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(Inventory inventory, CancellationToken cancellationToken = default);
    Task UpdateAsync(Inventory inventory, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Inventory, bool>> predicate, CancellationToken cancellationToken = default);
    Task ReserveQuantityAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task ReleaseReservedQuantityAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
}
