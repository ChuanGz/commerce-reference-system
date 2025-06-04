using ProductService.Application.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class CreateProductCommandHandler(IProductRepository repo)
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repo = repo;

    public async Task<Guid> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price,
            Category = request.Category.Trim(),
            SKU = request.SKU.Trim().ToUpper(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        await _repo.AddAsync(product, cancellationToken);
        return product.Id;
    }
}
