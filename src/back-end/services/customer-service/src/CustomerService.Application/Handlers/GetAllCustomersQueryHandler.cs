using CustomerService.Application.Queries;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers
{
    public class GetAllCustomersQueryHandler(ICustomerRepository repo)
        : IRequestHandler<GetAllCustomersQuery, List<Customer>>
    {
        public async Task<List<Customer>> Handle(
            GetAllCustomersQuery query,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetAllAsync(cancellationToken);
        }
    }
}
