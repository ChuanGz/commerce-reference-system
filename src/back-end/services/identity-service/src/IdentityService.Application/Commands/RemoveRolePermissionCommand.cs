namespace IdentityService.Application.Commands {
    public record RemoveRolePermissionCommand(Guid RoleId, Guid PermissionId) : IRequest<bool>;
}
