using CustomerService.Application.Queries;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers;

public class GetCustomerByIdQueryHandler(ICustomerRepository repo)
    : IRequestHandler<GetCustomerByIdQuery, Customer?>
{
    public async Task<Customer?> Handle(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        return await repo.GetByIdAsync(query.Id, cancellationToken);
    }
}
