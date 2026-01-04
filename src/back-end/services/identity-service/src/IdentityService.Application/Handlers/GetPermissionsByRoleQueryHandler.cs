using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers {
    public class GetPermissionsByRoleQueryHandler(IRoleRepository roleRepository)
        : IRequestHandler<GetPermissionsByRoleQuery, List<RolePermissionDto>> {
        public async Task<List<RolePermissionDto>> Handle(
            GetPermissionsByRoleQuery query,
            CancellationToken cancellationToken
        ) {
            ArgumentNullException.ThrowIfNull(query);
            var role = await roleRepository.GetByIdAsync(query.RoleId, cancellationToken);
            if (role is null || role.RolePermissions is null)
                return [];

            return role
                .RolePermissions.Select(rp => new RolePermissionDto(
                    rp.PermissionId,
                    rp.Permission?.Key ?? string.Empty,
                    rp.Permission?.Description
                ))
                .ToList();
        }
    }
}
