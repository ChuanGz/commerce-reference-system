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

public class TokenValidationResult
{
    public bool IsValid { get; set; }
    public string? UserId { get; set; }
    public IEnumerable<string> Permissions { get; set; } = [];
}
