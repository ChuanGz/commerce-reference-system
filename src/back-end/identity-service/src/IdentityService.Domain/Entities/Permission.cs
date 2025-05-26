namespace IdentityService.Domain.Entities;
public class Permission
{
    public Guid Id { get; set; }
    public string Key { get; set; } = default!;

    public string Description { get; set; } = string.Empty;
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}
