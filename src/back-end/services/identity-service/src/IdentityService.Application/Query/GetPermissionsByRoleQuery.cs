namespace IdentityService.Application.Queries {
    public record GetPermissionsByRoleQuery(Guid RoleId) : IRequest<List<RolePermissionDto>>;

    public record RolePermissionDto(Guid PermissionId, string Key, string? Description);
}
