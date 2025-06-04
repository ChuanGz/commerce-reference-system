using System.Linq.Expressions;
using CustomerService.Domain.Entities;

namespace CustomerService.Domain.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(
        Expression<Func<Customer, bool>> predicate,
        CancellationToken cancellationToken = default
    );
}
