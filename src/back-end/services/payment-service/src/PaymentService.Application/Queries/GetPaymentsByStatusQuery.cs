using PaymentService.Domain.Entities;

namespace PaymentService.Application.Queries
{
    public record GetPaymentsByStatusQuery(string Status) : IRequest<List<Payment>>;
}
