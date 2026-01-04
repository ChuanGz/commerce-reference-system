using CustomerService.Domain.Entities;

namespace CustomerService.Application.Queries {
    public record GetCustomerByIdQuery(Guid Id) : IRequest<Customer?>;
}
