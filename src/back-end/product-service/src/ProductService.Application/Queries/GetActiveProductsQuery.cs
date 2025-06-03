using ProductService.Domain.Entities;

namespace ProductService.Application.Queries;

public record GetActiveProductsQuery() : IRequest<List<Product>>;
