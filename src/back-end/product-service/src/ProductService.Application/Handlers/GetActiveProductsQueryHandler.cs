using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class GetActiveProductsQueryHandler(IProductRepository repo) : IRequestHandler<GetActiveProductsQuery, List<Product>>
{
    private readonly IProductRepository _repo = repo;

    public async Task<List<Product>> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken = default)
    {
        return await _repo.GetActiveProductsAsync(cancellationToken);
    }
}
