using System.Linq.Expressions;
using UserService.Domain.Entities;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(
        Expression<Func<User, bool>> value,
        CancellationToken cancellationToken = default
    );
}
