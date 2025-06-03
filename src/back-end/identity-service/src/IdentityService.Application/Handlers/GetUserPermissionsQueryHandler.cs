using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Handlers;

public class GetUserPermissionsQueryHandler(IUserRepository userRepository, ILogger<GetUserPermissionsQueryHandler> logger) : IRequestHandler<GetUserPermissionsQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting permissions for user: {UserId}", request.UserId);
        
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            logger.LogWarning("User not found: {UserId}", request.UserId);
            return [];
        }

        var permissions = user.UserGroups
            .SelectMany(ug => ug.Group.GroupRoles)
            .SelectMany(gr => gr.Role.RolePermissions)
            .Select(rp => rp.Permission.Key)
            .Distinct()
            .ToList();

        logger.LogInformation("Found {PermissionCount} permissions for user: {UserId}", permissions.Count, request.UserId);
        return permissions;
    }
}
