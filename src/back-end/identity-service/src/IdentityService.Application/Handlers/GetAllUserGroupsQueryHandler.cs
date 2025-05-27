using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class GetAllUserGroupsQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllUserGroupsQuery, List<UserGroupResponseDto>>
{
    public async Task<List<UserGroupResponseDto>> Handle(GetAllUserGroupsQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);

        return users.Select(u => new UserGroupResponseDto
        {
            UserId = u.Id,
            Username = u.Username,
            Groups = u.UserGroups.Select(ug => ug.Group?.Name ?? "").ToList()
        }).ToList();
    }
}
