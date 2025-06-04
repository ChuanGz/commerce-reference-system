using ProductService.Application.Commands;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class DeleteProductCommandHandler(IProductRepository repo)
    : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _repo = repo;

    public async Task<Unit> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
