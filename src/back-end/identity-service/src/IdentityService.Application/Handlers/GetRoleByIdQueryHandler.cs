using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    public async Task<RoleDto?> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(query.Id, cancellationToken);
        return role is null
            ? null
            : new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList(),
            };
    }
}
