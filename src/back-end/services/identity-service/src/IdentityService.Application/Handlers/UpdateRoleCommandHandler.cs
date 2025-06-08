using IdentityService.Application.Commands;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class UpdateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<UpdateRoleCommand, bool>
{
    public async Task<bool> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        var role = await roleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (role is null)
            return false;

        role.Name = command.Name;
        role.RolePermissions.Clear();

        foreach (var pid in command.PermissionIds)
            role.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = pid });

        await roleRepository.UpdateAsync(role, cancellationToken);
        return true;
    }
}
