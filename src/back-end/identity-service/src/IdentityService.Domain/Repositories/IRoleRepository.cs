using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Role role, CancellationToken cancellationToken = default);
    Task UpdateAsync(Role role, CancellationToken cancellationToken = default);
    Task DeleteAsync(Role role, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken = default
    );
    Task AssignPermissionAsync(
        RolePermission rolePermission,
        CancellationToken cancellationToken = default
    );
    Task<bool> RevokePermissionAsync(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken = default
    );
}
