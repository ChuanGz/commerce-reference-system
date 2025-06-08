using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories;

public class UserGroupRepository(IdentityDbContext db) : IUserGroupRepository
{
    public async Task<UserGroup?> GetAsync(
        Guid userId,
        Guid groupId,
        CancellationToken cancellationToken = default
    ) =>
        await db.UserGroups.FirstOrDefaultAsync(
            ug => ug.UserId == userId && ug.GroupId == groupId,
            cancellationToken
        );

    public async Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default)
    {
        db.UserGroups.Add(userGroup);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task ApproveAsync(
        UserGroup userGroup,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userGroup, nameof(userGroup));
        userGroup.IsApproved = true;
        db.UserGroups.Update(userGroup);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(
        UserGroup userGroup,
        CancellationToken cancellationToken = default
    )
    {
        db.UserGroups.Remove(userGroup);
        await db.SaveChangesAsync(cancellationToken);
    }
}
