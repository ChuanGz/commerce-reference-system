namespace ProductService.Application.Commands
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        string Category,
        string SKU
    ) : IRequest<Guid>;
}
