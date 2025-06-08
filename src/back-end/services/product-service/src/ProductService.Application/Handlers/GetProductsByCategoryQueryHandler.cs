using ProductService.Application.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers
{
    public class GetProductsByCategoryQueryHandler(IProductRepository repo)
        : IRequestHandler<GetProductsByCategoryQuery, List<Product>>
    {
        public async Task<List<Product>> Handle(
            GetProductsByCategoryQuery query,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetByCategoryAsync(query.Category, cancellationToken);
        }
    }
}
