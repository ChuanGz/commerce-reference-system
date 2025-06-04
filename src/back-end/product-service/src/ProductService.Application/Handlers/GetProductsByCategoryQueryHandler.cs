using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class GetProductsByCategoryQueryHandler(IProductRepository repo)
    : IRequestHandler<GetProductsByCategoryQuery, List<Product>>
{
    private readonly IProductRepository _repo = repo;

    public async Task<List<Product>> Handle(
        GetProductsByCategoryQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetByCategoryAsync(request.Category, cancellationToken);
    }
}
