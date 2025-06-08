using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories
{
    public class PermissionRepository(IdentityDbContext db) : IPermissionRepository
    {
        public async Task<List<Permission>> GetAllAsync(
            CancellationToken cancellationToken = default
        ) => await db.Permissions.ToListAsync(cancellationToken);

        public async Task<Permission?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default
        ) => await db.Permissions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public async Task AddAsync(
            Permission permission,
            CancellationToken cancellationToken = default
        )
        {
            db.Permissions.Add(permission);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            Guid id,
            CancellationToken cancellationToken = default
        ) => await db.Permissions.AnyAsync(p => p.Id == id, cancellationToken);
    }
}
