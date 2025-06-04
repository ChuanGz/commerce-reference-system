using IdentityService.Application.Commands;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class RemoveRolePermissionCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<RemoveRolePermissionCommand, bool>
{
    public async Task<bool> Handle(
        RemoveRolePermissionCommand command,
        CancellationToken cancellationToken
    )
    {
        var removed = await roleRepository.RevokePermissionAsync(
            command.RoleId,
            command.PermissionId,
            cancellationToken
        );
        return removed;
    }
}
