namespace IdentityService.Application.Commands;

public record CreateRoleCommand(string Name, List<Guid> PermissionIds) : IRequest<Guid>;
