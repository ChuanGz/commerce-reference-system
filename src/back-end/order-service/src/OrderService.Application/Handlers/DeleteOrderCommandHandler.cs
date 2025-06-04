using OrderService.Application.Commands;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class DeleteOrderCommandHandler(IOrderRepository repo)
    : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<Unit> Handle(
        DeleteOrderCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
