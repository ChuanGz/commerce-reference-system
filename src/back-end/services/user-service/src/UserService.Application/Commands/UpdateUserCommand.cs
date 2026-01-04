namespace UserService.Application.Commands {
    public record UpdateUserCommand(Guid Id, string Name, string Email) : IRequest;
}
