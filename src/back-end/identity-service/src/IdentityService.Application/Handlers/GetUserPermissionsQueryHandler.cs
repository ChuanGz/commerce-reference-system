using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Handlers;

public class GetUserPermissionsQueryHandler(
    IUserRepository userRepository,
    ILogger<GetUserPermissionsQueryHandler> logger
) : IRequestHandler<GetUserPermissionsQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(
        GetUserPermissionsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        logger.LogInformation("Getting permissions for user: {UserId}", query.UserId);

        var user = await userRepository.GetByIdAsync(query.UserId, cancellationToken);
        if (user == null)
        {
            logger.LogWarning("User not found: {UserId}", query.UserId);
            return [];
        }

        var permissions = user
            .UserGroups.SelectMany(ug => ug.Group.GroupRoles)
            .SelectMany(gr => gr.Role.RolePermissions)
            .Select(rp => rp.Permission.Key)
            .Distinct()
            .ToList();

        logger.LogInformation(
            "Found {PermissionCount} permissions for user: {UserId}",
            permissions.Count,
            query.UserId
        );
        return permissions;
    }
}
