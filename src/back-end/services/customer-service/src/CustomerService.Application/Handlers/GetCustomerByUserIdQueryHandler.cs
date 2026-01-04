using CustomerService.Application.Queries;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Repositories;

namespace CustomerService.Application.Handlers {
    public class GetCustomerByUserIdQueryHandler(ICustomerRepository repo)
        : IRequestHandler<GetCustomerByUserIdQuery, Customer?> {
        public async Task<Customer?> Handle(
            GetCustomerByUserIdQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);
            return await repo.GetByUserIdAsync(query.UserId, cancellationToken);
        }
    }
}
