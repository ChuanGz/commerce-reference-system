using OrderService.Domain.Entities;

namespace OrderService.Application.Queries;

public record GetOrdersByCustomerIdQuery(Guid CustomerId) : IRequest<List<Order>>;
