using IdentityService.Application.Commands;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class RemoveUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    : IRequestHandler<RemoveUserGroupCommand, bool>
{
    public async Task<bool> Handle(
        RemoveUserGroupCommand command,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(command);
        var userGroup = await userGroupRepository.GetAsync(
            command.UserId,
            command.GroupId,
            cancellationToken
        );
        if (userGroup is null)
            return false;

        await userGroupRepository.RemoveAsync(userGroup, cancellationToken);
        return true;
    }
}
