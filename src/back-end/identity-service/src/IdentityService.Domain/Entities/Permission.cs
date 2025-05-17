namespace IdentityService.Domain.Entities;

/// <summary>
/// A permission represents a low-level capability like CAN_VIEW_USER or CAN_EDIT_INVOICE.
/// Assigned to roles.
/// </summary>
public class Permission
{
    public Guid Id { get; set; }

    /// <summary>
    /// Unique key identifier (e.g. "CAN_VIEW_USER"). Used in JWT claims and policies.
    /// </summary>
    public string Key { get; set; } = default!;

    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Roles that include this permission.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
