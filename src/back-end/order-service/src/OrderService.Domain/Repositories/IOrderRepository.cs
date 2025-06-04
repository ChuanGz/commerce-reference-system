using System.Linq.Expressions;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default
    );
    Task<List<Order>> GetByStatusAsync(
        string status,
        CancellationToken cancellationToken = default
    );
    Task<List<Order>> GetOrdersByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(
        Expression<Func<Order, bool>> predicate,
        CancellationToken cancellationToken = default
    );
}
