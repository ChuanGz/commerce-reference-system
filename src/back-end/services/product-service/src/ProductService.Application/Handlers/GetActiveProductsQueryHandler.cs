using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers
{
    public class GetActiveProductsQueryHandler(IProductRepository repo)
        : IRequestHandler<GetActiveProductsQuery, List<Product>>
    {
        public async Task<List<Product>> Handle(
            GetActiveProductsQuery query,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetActiveProductsAsync(cancellationToken);
        }
    }
}
