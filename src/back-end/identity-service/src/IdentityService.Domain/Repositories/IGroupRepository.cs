using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Group group, CancellationToken cancellationToken = default);
    Task UpdateAsync(Group group, CancellationToken cancellationToken = default);
    Task DeleteAsync(Group group, CancellationToken cancellationToken = default);
}
