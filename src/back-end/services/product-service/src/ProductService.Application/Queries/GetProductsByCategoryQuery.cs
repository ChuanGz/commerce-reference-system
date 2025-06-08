using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public record GetProductsByCategoryQuery(string Category) : IRequest<List<Product>>;
}
