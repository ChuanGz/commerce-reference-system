using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Product?>;
}
