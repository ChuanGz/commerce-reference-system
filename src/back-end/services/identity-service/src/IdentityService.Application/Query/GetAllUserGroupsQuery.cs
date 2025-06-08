using IdentityService.Application.Models;

namespace IdentityService.Application.Queries;

public record GetAllUserGroupsQuery() : IRequest<List<UserGroupResponseDto>>;
