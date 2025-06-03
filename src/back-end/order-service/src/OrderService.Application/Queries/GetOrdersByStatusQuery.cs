using OrderService.Domain.Entities;

namespace OrderService.Application.Queries;

public record GetOrdersByStatusQuery(string Status) : IRequest<List<Order>>;
