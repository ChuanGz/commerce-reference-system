using ProductService.Application.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers
{
    public class CreateProductCommandHandler(IProductRepository repo)
        : IRequestHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(
            CreateProductCommand command,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(command);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name.Trim(),
                Description = command.Description.Trim(),
                Price = command.Price,
                Category = command.Category.Trim(),
                SKU = command.SKU.Trim().ToUpper(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            await repo.AddAsync(product, cancellationToken);
            return product.Id;
        }
    }
}
