namespace IdentityService.Application.Commands
{
    public record UpdateRoleCommand(Guid Id, string Name, List<Guid> PermissionIds)
        : IRequest<bool>;
}
