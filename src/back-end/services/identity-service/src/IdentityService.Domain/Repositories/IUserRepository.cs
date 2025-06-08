using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        );
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> ExistsByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
