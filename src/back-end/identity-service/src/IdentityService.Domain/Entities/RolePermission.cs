namespace IdentityService.Domain.Entities;

/// <summary>
/// Join table: Role <-> Permission (many-to-many).
/// </summary>
public class RolePermission
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;
}
