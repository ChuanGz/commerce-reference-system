using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Application.Commands;
using IdentityService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Handlers;

public class AuthenticateUserCommandHandler(
    IAuthenticationService authService,
    ILogger<AuthenticateUserCommandHandler> logger
) : IRequestHandler<AuthenticateUserCommand, string?>
{
    public async Task<string?> Handle(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(command);

        logger.LogInformation(
            "Processing authentication request for user: {Username}",
            command.Username
        );

        try
        {
            var result = await authService.AuthenticateAsync(
                command.Username,
                command.Password,
                cancellationToken
            );

            if (result != null)
            {
                logger.LogInformation(
                    "Authentication successful for user: {Username}",
                    command.Username
                );
            }
            else
            {
                logger.LogWarning("Authentication failed for user: {Username}", command.Username);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error during authentication for user: {Username}",
                command.Username
            );
            throw;
        }
    }
}
