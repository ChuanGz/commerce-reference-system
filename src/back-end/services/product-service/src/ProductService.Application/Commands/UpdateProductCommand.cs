namespace ProductService.Application.Commands
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string Category,
        bool IsActive
    ) : IRequest<Unit>;
}
