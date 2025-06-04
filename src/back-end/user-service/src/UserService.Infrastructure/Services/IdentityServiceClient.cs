using System.Text.Json;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Services;

public class IdentityServiceClient(HttpClient httpClient, ILogger<IdentityServiceClient> logger)
    : IIdentityServiceClient
{
    public async Task<bool> ValidateUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation("Validating user {UserId} with Identity Service", userId);
            var response = await httpClient.GetAsync(
                $"/api/users/{userId}/validate",
                cancellationToken
            );
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to validate user {UserId} with Identity Service", userId);
            return false;
        }
    }

    public async Task<UserPermissions> GetUserPermissionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation(
                "Getting permissions for user {UserId} from Identity Service",
                userId
            );
            var response = await httpClient.GetAsync(
                $"/api/users/{userId}/permissions",
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to get permissions for user {UserId}", userId);
                return new UserPermissions();
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var permissions = JsonSerializer.Deserialize<UserPermissions>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return permissions ?? new UserPermissions();
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to get permissions for user {UserId} from Identity Service",
                userId
            );
            return new UserPermissions();
        }
    }
}
