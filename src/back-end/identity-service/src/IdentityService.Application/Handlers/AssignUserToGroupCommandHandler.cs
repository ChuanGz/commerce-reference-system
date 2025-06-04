using IdentityService.Application.Commands;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class AssignUserToGroupCommandHandler(IUserGroupRepository userGroupRepository)
    : IRequestHandler<AssignUserToGroupCommand, bool>
{
    public async Task<bool> Handle(
        AssignUserToGroupCommand command,
        CancellationToken cancellationToken
    )
    {
        var exists = await userGroupRepository.GetAsync(
            command.UserId,
            command.GroupId,
            cancellationToken
        );
        if (exists is not null)
            return false;

        var userGroup = new UserGroup
        {
            UserId = command.UserId,
            GroupId = command.GroupId,
            IsApproved = false,
        };

        await userGroupRepository.AddAsync(userGroup, cancellationToken);
        return true;
    }
}
