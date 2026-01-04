using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories {
    public interface IUserGroupRepository {
        Task<UserGroup?> GetAsync(
            Guid userId,
            Guid groupId,
            CancellationToken cancellationToken = default
        );
        Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default);
        Task ApproveAsync(UserGroup userGroup, CancellationToken cancellationToken = default);
        Task RemoveAsync(UserGroup userGroup, CancellationToken cancellationToken = default);
    }
}
