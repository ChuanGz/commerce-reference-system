using CustomerService.Application.Commands;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers;

public class DeleteCustomerCommandHandler(ICustomerRepository repo)
    : IRequestHandler<DeleteCustomerCommand, Unit>
{
    private readonly ICustomerRepository _repo = repo;

    public async Task<Unit> Handle(
        DeleteCustomerCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        await _repo.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
