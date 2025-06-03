using OrderService.Application.Commands;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class UpdateOrderStatusCommandHandler(IOrderRepository repo) : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderRepository _repo = repo;

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (order == null)
            return Unit.Value;

        order.Status = request.Status.Trim();

        await _repo.UpdateAsync(order, cancellationToken);
        return Unit.Value;
    }
}
