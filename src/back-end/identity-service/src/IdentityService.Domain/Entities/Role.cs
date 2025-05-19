namespace IdentityService.Domain.Entities;

/// <summary>
/// Roles define job functions like Admin, Viewer, or HR Manager.
/// Roles are assigned to groups and linked to permissions.
/// </summary>
public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    /// <summary>
    /// Groups that have this role.
    /// </summary>
    public ICollection<GroupRole> GroupRoles { get; set; } = [];

    /// <summary>
    /// Permissions that this role grants.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}
