using CustomerService.Application.Queries;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers;

public class GetAllCustomersQueryHandler(ICustomerRepository repo)
    : IRequestHandler<GetAllCustomersQuery, List<Customer>>
{
    private readonly ICustomerRepository _repo = repo;

    public async Task<List<Customer>> Handle(
        GetAllCustomersQuery request,
        CancellationToken cancellationToken = default
    )
    {
        return await _repo.GetAllAsync(cancellationToken);
    }
}
