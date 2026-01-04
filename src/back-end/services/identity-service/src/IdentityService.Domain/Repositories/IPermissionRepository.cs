using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories {
    public interface IPermissionRepository {
        Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
