using IdentityService.Application.Commands;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class AddRolePermissionCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<AddRolePermissionCommand, bool>
{
    public async Task<bool> Handle(
        AddRolePermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(command);

        var hasPermission = await roleRepository.HasPermissionAsync(
            command.RoleId,
            command.PermissionId,
            cancellationToken
        );
        if (hasPermission)
            return false;

        var rolePermission = new RolePermission
        {
            RoleId = command.RoleId,
            PermissionId = command.PermissionId,
        };

        await roleRepository.AssignPermissionAsync(rolePermission, cancellationToken);
        return true;
    }
}
