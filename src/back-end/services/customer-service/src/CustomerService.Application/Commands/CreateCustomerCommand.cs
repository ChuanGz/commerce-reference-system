namespace CustomerService.Application.Commands;

public record CreateCustomerCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Phone,
    string Address
) : IRequest<Guid>;
