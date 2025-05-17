using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

public class UserGroupRepository : IUserGroupRepository
{
    private readonly IdentityDbContext _db;

    public UserGroupRepository(IdentityDbContext db) => _db = db;

    public async Task<UserGroup?> GetAsync(
        Guid userId,
        Guid groupId,
        CancellationToken cancellationToken = default
    ) =>
        await _db.UserGroups.FirstOrDefaultAsync(
            ug => ug.UserId == userId && ug.GroupId == groupId,
            cancellationToken
        );

    public async Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default)
    {
        _db.UserGroups.Add(userGroup);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task ApproveAsync(
        UserGroup userGroup,
        CancellationToken cancellationToken = default
    )
    {
        userGroup.IsApproved = true;
        _db.UserGroups.Update(userGroup);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(
        UserGroup userGroup,
        CancellationToken cancellationToken = default
    )
    {
        _db.UserGroups.Remove(userGroup);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
