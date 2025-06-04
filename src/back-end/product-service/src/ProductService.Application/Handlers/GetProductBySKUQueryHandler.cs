using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class GetProductBySKUQueryHandler(IProductRepository repo)
    : IRequestHandler<GetProductBySKUQuery, Product?>
{
    private readonly IProductRepository _repo = repo;

    public async Task<Product?> Handle(
        GetProductBySKUQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repo.GetBySKUAsync(query.SKU, cancellationToken);
    }
}
