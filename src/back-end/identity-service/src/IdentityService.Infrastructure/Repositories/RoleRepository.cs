using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories;

public class RoleRepository(IdentityDbContext db) : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        db.Roles.Add(role);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Role role, CancellationToken cancellationToken = default)
    {
        db.Roles.Update(role);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Role role, CancellationToken cancellationToken = default)
    {
        db.Roles.Remove(role);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Roles.AnyAsync(r => r.Id == id, cancellationToken);
    }
}
