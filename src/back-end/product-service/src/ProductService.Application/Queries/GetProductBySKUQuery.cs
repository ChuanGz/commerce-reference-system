using ProductService.Domain.Entities;

namespace ProductService.Application.Queries;

public record GetProductBySKUQuery(string SKU) : IRequest<Product?>;
