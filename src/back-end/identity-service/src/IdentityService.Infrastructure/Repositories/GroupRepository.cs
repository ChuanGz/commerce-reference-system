using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories;

public class GroupRepository(IdentityDbContext db) : IGroupRepository
{
    public async Task<Group?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) =>
        await db.Groups
            .Include(g => g.GroupRoles)
            .ThenInclude(gr => gr.Role)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.Groups
            .Include(g => g.GroupRoles)
            .ThenInclude(gr => gr.Role)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
    {
        db.Groups.Add(group);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        db.Groups.Update(group);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Group group, CancellationToken cancellationToken = default)
    {
        db.Groups.Remove(group);
        await db.SaveChangesAsync(cancellationToken);
    }
}
