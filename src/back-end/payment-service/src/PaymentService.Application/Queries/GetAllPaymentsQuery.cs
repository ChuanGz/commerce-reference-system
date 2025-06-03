using PaymentService.Domain.Entities;

namespace PaymentService.Application.Queries;

public record GetAllPaymentsQuery() : IRequest<List<Payment>>;
