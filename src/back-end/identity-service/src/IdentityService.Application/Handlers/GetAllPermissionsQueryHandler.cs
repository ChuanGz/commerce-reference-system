using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class GetAllPermissionsQueryHandler
    : IRequestHandler<GetAllPermissionsQuery, List<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<List<PermissionDto>> Handle(
        GetAllPermissionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var permissions = await _permissionRepository.GetAllAsync(cancellationToken);

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
