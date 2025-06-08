using System.Linq.Expressions;
using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Repositories;

public interface IPaymentRepository
{
    Task<List<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<Payment?> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken cancellationToken = default
    );
    Task<List<Payment>> GetByStatusAsync(
        string status,
        CancellationToken cancellationToken = default
    );
    Task<List<Payment>> GetByPaymentMethodAsync(
        string paymentMethod,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(
        Expression<Func<Payment, bool>> predicate,
        CancellationToken cancellationToken = default
    );
}
