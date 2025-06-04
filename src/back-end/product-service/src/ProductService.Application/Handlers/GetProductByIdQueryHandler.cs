using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class GetProductByIdQueryHandler(IProductRepository repo)
    : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductRepository _repo = repo;

    public async Task<Product?> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repo.GetByIdAsync(query.Id, cancellationToken);
    }
}
