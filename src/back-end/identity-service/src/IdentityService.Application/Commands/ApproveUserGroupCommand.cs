namespace IdentityService.Application.Commands;

public record ApproveUserGroupCommand(Guid UserId, Guid GroupId) : IRequest<bool>;
