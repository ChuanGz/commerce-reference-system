using CustomerService.Application.Queries;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers;

public class GetCustomerByUserIdQueryHandler(ICustomerRepository repo) : IRequestHandler<GetCustomerByUserIdQuery, Customer?>
{
    private readonly ICustomerRepository _repo = repo;

    public async Task<Customer?> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken = default)
    {
        return await _repo.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}
