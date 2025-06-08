namespace OrderService.Application.Commands;

public record DeleteOrderCommand(Guid Id) : IRequest<Unit>;
