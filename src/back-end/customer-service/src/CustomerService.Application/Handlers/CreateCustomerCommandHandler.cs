using CustomerService.Application.Commands;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers;

public class CreateCustomerCommandHandler(ICustomerRepository repo)
    : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repo = repo;

    public async Task<Guid> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Phone = request.Phone.Trim(),
            Address = request.Address.Trim(),
            CreatedAt = DateTime.UtcNow,
        };

        await _repo.AddAsync(customer, cancellationToken);
        return customer.Id;
    }
}
