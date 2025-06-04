using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class GetProductBySKUQueryHandler(IProductRepository repo)
    : IRequestHandler<GetProductBySKUQuery, Product?>
{
    private readonly IProductRepository _repo = repo;

    public async Task<Product?> Handle(
        GetProductBySKUQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetBySKUAsync(request.SKU, cancellationToken);
    }
}
