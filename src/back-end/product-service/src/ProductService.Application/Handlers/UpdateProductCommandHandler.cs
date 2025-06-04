using ProductService.Application.Commands;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class UpdateProductCommandHandler(IProductRepository repo)
    : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _repo = repo;

    public async Task<Unit> Handle(
        UpdateProductCommand query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);

        var product = await _repo.GetByIdAsync(query.Id, cancellationToken);
        if (product == null)
            return Unit.Value;

        product.Name = query.Name.Trim();
        product.Description = query.Description.Trim();
        product.Price = query.Price;
        product.Category = query.Category.Trim();
        product.IsActive = query.IsActive;

        await _repo.UpdateAsync(product, cancellationToken);
        return Unit.Value;
    }
}
