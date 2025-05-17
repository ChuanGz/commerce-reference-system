using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly IdentityDbContext _db;

    public PermissionRepository(IdentityDbContext db) => _db = db;

    public async Task<List<Permission>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await _db.Permissions.ToListAsync(cancellationToken);

    public async Task<Permission?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) => await _db.Permissions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        _db.Permissions.Add(permission);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Permissions.AnyAsync(p => p.Id == id, cancellationToken);
}
