using IdentityService.Application.Commands;
using IdentityService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Handlers;

public class AuthenticateUserCommandHandler(IAuthenticationService authService, ILogger<AuthenticateUserCommandHandler> logger) : IRequestHandler<AuthenticateUserCommand, string?>
{
    public async Task<string?> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Processing authentication request for user: {Username}", request.Username);
        
        try
        {
            var result = await authService.AuthenticateAsync(request.Username, request.Password, cancellationToken);
            
            if (result != null)
            {
                logger.LogInformation("Authentication successful for user: {Username}", request.Username);
            }
            else
            {
                logger.LogWarning("Authentication failed for user: {Username}", request.Username);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during authentication for user: {Username}", request.Username);
            throw;
        }
    }
}
