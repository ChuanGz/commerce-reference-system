namespace UserService.Application.Commands {
    public record CreateUserCommand(string Name, string Email) : IRequest<Guid>;
}
