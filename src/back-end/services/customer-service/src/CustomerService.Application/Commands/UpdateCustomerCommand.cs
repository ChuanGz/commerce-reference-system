namespace CustomerService.Application.Commands;

public record UpdateCustomerCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Address
) : IRequest<Unit>;
