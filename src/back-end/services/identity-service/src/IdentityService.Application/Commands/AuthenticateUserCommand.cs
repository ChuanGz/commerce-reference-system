namespace IdentityService.Application.Commands
{
    public record AuthenticateUserCommand(string Username, string Password) : IRequest<string?>;
}
