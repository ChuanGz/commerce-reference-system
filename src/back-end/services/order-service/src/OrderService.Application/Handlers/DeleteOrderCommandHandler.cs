using OrderService.Application.Commands;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class DeleteOrderCommandHandler(IOrderRepository repo)
    : IRequestHandler<DeleteOrderCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteOrderCommand command,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(command);

        await repo.DeleteAsync(command.Id, cancellationToken);
        return Unit.Value;
    }
}
