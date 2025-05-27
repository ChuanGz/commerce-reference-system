using IdentityService.Application.Commands;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class CreateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<CreateRoleCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            RolePermissions = command.PermissionIds
                .Select(pid => new RolePermission { PermissionId = pid })
                .ToList()
        };

        await roleRepository.AddAsync(role, cancellationToken);
        return role.Id;
    }
}
