namespace IdentityService.Application.Commands;

public record AddRolePermissionCommand(Guid RoleId, Guid PermissionId) : IRequest<bool>;
