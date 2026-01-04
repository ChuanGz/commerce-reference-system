namespace IdentityService.Application.Commands {
    public record AssignUserToGroupCommand(Guid UserId, Guid GroupId) : IRequest<bool>;
}
