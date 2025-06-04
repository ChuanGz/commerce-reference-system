using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class DeleteInventoryCommandHandler(IInventoryRepository repo)
    : IRequestHandler<DeleteInventoryCommand, Unit>
{
    private readonly IInventoryRepository _repo = repo;

    public async Task<Unit> Handle(
        DeleteInventoryCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
