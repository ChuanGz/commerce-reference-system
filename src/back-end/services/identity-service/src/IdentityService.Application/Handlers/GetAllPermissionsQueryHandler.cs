using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
    : IRequestHandler<GetAllPermissionsQuery, List<PermissionDto>>
{
    public async Task<List<PermissionDto>> Handle(
        GetAllPermissionsQuery command,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(command);

        var permissions = await permissionRepository.GetAllAsync(cancellationToken);

        return permissions
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Key = p.Key,
                Description = p.Description,
            })
            .ToList();
    }
}
