using OrderService.Domain.Entities;

namespace OrderService.Application.Queries;

public record GetOrdersByDateRangeQuery(DateTime StartDate, DateTime EndDate)
    : IRequest<List<Order>>;
