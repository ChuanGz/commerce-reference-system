using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;
using PaymentService.Infrastructure.Persistence;

namespace PaymentService.Infrastructure.Repositories;

public class PaymentRepository(PaymentDbContext context) : IPaymentRepository
{
    private readonly PaymentDbContext _context = context;

    public async Task<List<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Payments.ToListAsync(cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.FindAsync([id], cancellationToken);
    }

    public async Task<Payment?> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Payments.FirstOrDefaultAsync(
            p => p.OrderId == orderId,
            cancellationToken
        );
    }

    public async Task<Payment?> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Payments.FirstOrDefaultAsync(
            p => p.TransactionId == transactionId,
            cancellationToken
        );
    }

    public async Task<List<Payment>> GetByStatusAsync(
        string status,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Payments.Where(p => p.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Payment>> GetByPaymentMethodAsync(
        string paymentMethod,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Payments.Where(p => p.PaymentMethod == paymentMethod)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await GetByIdAsync(id, cancellationToken);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> AnyAsync(
        Expression<Func<Payment, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Payments.AnyAsync(predicate, cancellationToken);
    }
}
