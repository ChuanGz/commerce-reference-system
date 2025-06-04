using System.Linq.Expressions;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using InventoryService.Domain.Constants;

namespace InventoryService.Infrastructure.Repositories;

public class InventoryRepository(InventoryDbContext context) : IInventoryRepository
{
    private readonly InventoryDbContext _context = context;

    public async Task<List<Inventory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Inventories.ToListAsync(cancellationToken);
    }

    public async Task<Inventory?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Inventories.FindAsync([id], cancellationToken);
    }

    public async Task<Inventory?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Inventories.FirstOrDefaultAsync(
            i => i.ProductId == productId,
            cancellationToken
        );
    }

    public async Task AddAsync(Inventory inventory, CancellationToken cancellationToken = default)
    {
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Inventory inventory,
        CancellationToken cancellationToken = default
    )
    {
        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inventory = await GetByIdAsync(id, cancellationToken);
        if (inventory != null)
        {
            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> AnyAsync(
        Expression<Func<Inventory, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Inventories.AnyAsync(predicate, cancellationToken);
    }

    public async Task ReserveQuantityAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    )
    {
        var inventory = await GetByProductIdAsync(productId, cancellationToken);
        if (inventory != null && inventory.Quantity >= quantity)
        {
            inventory.ReservedQuantity += quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            await UpdateAsync(inventory, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException(ErrorMessages.InsufficientInventory);
        }
    }

    public async Task ReleaseReservedQuantityAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    )
    {
        var inventory = await GetByProductIdAsync(productId, cancellationToken);
        if (inventory != null && inventory.ReservedQuantity >= quantity)
        {
            inventory.ReservedQuantity -= quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            await UpdateAsync(inventory, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException(ErrorMessages.InsufficientReservedQuantity);
        }
    }
}
