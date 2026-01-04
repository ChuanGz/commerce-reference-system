using CustomerService.Domain.Entities;

namespace CustomerService.Application.Queries {
    public record GetAllCustomersQuery() : IRequest<List<Customer>>;
}
