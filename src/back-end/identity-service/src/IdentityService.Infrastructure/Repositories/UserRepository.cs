using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _db;

    public UserRepository(IdentityDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default
    )
    {
        return await _db
            .Users.Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.GroupRoles)
            .ThenInclude(gr => gr.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db
            .Users.Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.GroupRoles)
            .ThenInclude(gr => gr.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Users.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        _db.Users.Remove(user);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default
    )
    {
        return await _db.Users.AnyAsync(u => u.Username == username, cancellationToken);
    }
}
