using ProductService.Application.Commands;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class UpdateProductCommandHandler(IProductRepository repo)
    : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _repo = repo;

    public async Task<Unit> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            return Unit.Value;

        product.Name = request.Name.Trim();
        product.Description = request.Description.Trim();
        product.Price = request.Price;
        product.Category = request.Category.Trim();
        product.IsActive = request.IsActive;

        await _repo.UpdateAsync(product, cancellationToken);
        return Unit.Value;
    }
}
