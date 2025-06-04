using CustomerService.Application.Queries;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers;

public class GetCustomerByIdQueryHandler(ICustomerRepository repo)
    : IRequestHandler<GetCustomerByIdQuery, Customer?>
{
    private readonly ICustomerRepository _repo = repo;

    public async Task<Customer?> Handle(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repo.GetByIdAsync(query.Id, cancellationToken);
    }
}
