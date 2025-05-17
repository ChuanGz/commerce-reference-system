using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IdentityDbContext _db;

    public GroupRepository(IdentityDbContext db) => _db = db;

    public async Task<Group?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) =>
        await _db.Groups
            .Include(g => g.GroupRoles)
            .ThenInclude(gr => gr.Role)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.Groups
            .Include(g => g.GroupRoles)
            .ThenInclude(gr => gr.Role)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
    {
        _db.Groups.Add(group);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        _db.Groups.Update(group);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Group group, CancellationToken cancellationToken = default)
    {
        _db.Groups.Remove(group);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
