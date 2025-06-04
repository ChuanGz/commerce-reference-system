using IdentityService.Application.Commands;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class DeleteRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<DeleteRoleCommand, bool>
{
    public async Task<bool> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (role is null)
            return false;

        await roleRepository.DeleteAsync(role, cancellationToken);
        return true;
    }
}
