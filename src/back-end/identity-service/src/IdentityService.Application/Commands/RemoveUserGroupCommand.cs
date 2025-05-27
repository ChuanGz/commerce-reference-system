namespace IdentityService.Application.Commands;

public record RemoveUserGroupCommand(Guid UserId, Guid GroupId) : IRequest<bool>;
