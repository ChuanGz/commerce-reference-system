using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories
{
    public class RoleRepository(IdentityDbContext db) : IRoleRepository
    {
        public async Task<Role?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default
        )
        {
            return await db
                .Roles.Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await db
                .Roles.Include(r => r.RolePermissions)
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

        public async Task<bool> HasPermissionAsync(
            Guid roleId,
            Guid permissionId,
            CancellationToken cancellationToken = default
        )
        {
            return await db.RolePermissions.AnyAsync(
                rp => rp.RoleId == roleId && rp.PermissionId == permissionId,
                cancellationToken
            );
        }

        public async Task AssignPermissionAsync(
            RolePermission rolePermission,
            CancellationToken cancellationToken = default
        )
        {
            db.RolePermissions.Add(rolePermission);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RevokePermissionAsync(
            Guid roleId,
            Guid permissionId,
            CancellationToken cancellationToken = default
        )
        {
            var existing = await db.RolePermissions.FirstOrDefaultAsync(
                rp => rp.RoleId == roleId && rp.PermissionId == permissionId,
                cancellationToken
            );

            if (existing is null)
                return false;

            db.RolePermissions.Remove(existing);
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
