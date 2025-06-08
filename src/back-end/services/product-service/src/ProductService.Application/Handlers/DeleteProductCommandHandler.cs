using ProductService.Application.Commands;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class DeleteProductCommandHandler(IProductRepository repo)
    : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteProductCommand command,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(command);

        await repo.DeleteAsync(command.Id, cancellationToken);
        return Unit.Value;
    }
}
