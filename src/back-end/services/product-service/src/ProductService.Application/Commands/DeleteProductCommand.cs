namespace ProductService.Application.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;
