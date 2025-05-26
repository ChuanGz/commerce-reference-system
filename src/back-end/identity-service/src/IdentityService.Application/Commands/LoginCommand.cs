namespace IdentityService.Application.Commands;
public record LoginCommand(string Username, string Password) : IRequest<string?>;
