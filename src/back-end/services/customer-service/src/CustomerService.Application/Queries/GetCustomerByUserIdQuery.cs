using CustomerService.Domain.Entities;

namespace CustomerService.Application.Queries {
    public record GetCustomerByUserIdQuery(Guid UserId) : IRequest<Customer?>;
}
