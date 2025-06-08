using IdentityService.Application.Models;

namespace IdentityService.Application.Interfaces;

public interface IAuthenticationService
{
    Task<string?> AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default
    );
    Task<TokenValidationResult> ValidateTokenAsync(
        string token,
        CancellationToken cancellationToken = default
    );
    Task RevokeTokenAsync(string token, CancellationToken cancellationToken = default);
}
