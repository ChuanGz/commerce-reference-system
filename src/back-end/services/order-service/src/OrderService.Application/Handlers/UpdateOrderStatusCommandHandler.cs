using OrderService.Application.Commands;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers
{
    public class UpdateOrderStatusCommandHandler(IOrderRepository repo)
        : IRequestHandler<UpdateOrderStatusCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateOrderStatusCommand command,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(command);

            var order = await repo.GetByIdAsync(command.Id, cancellationToken);
            if (order == null)
                return Unit.Value;

            order.Status = command.Status.Trim();

            await repo.UpdateAsync(order, cancellationToken);
            return Unit.Value;
        }
    }
}
