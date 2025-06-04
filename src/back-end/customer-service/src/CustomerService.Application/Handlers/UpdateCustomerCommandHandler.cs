using CustomerService.Application.Commands;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers;

public class UpdateCustomerCommandHandler(ICustomerRepository repo)
    : IRequestHandler<UpdateCustomerCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (customer == null)
            return Unit.Value;

        customer.FirstName = request.FirstName.Trim();
        customer.LastName = request.LastName.Trim();
        customer.Phone = request.Phone.Trim();
        customer.Address = request.Address.Trim();

        await repo.UpdateAsync(customer, cancellationToken);
        return Unit.Value;
    }
}
