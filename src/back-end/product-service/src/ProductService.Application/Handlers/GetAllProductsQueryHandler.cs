using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class GetAllProductsQueryHandler(IProductRepository repo)
    : IRequestHandler<GetAllProductsQuery, List<Product>>
{
    private readonly IProductRepository _repo = repo;

    public async Task<List<Product>> Handle(
        GetAllProductsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repo.GetAllAsync(cancellationToken);
    }
}
