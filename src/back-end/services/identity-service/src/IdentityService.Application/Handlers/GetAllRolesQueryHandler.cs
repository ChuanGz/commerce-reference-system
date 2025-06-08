using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers
{
    public class GetAllRolesQueryHandler(IRoleRepository roleRepository)
        : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
    {
        public async Task<List<RoleDto>> Handle(
            GetAllRolesQuery query,
            CancellationToken cancellationToken
        )
        {
            ArgumentNullException.ThrowIfNull(query);

            var roles = await roleRepository.GetAllAsync(cancellationToken);

            return roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    PermissionIds = r.RolePermissions.Select(rp => rp.PermissionId).ToList(),
                })
                .ToList();
        }
    }
}
