using IdentityService.Application.Commands;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class ApproveUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    : IRequestHandler<ApproveUserGroupCommand, bool>
{
    public async Task<bool> Handle(
        ApproveUserGroupCommand command,
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

        userGroup.IsApproved = true;
        await userGroupRepository.ApproveAsync(userGroup, cancellationToken);
        return true;
    }
}
