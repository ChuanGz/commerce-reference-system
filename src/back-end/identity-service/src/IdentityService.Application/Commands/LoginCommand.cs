namespace IdentityService.Application.Commands;

/// <summary>
/// Login command for validating user credentials and issuing a JWT token.
/// </summary>
public record LoginCommand(string Username, string Password) : IRequest<string?>;
