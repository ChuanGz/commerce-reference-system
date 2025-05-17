namespace IdentityService.Domain.Entities;

/// <summary>
/// A group is a collection of users, like departments or teams.
/// Groups are mapped to roles.
/// </summary>
public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    /// <summary>
    /// Users that belong to this group.
    /// </summary>
    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();

    /// <summary>
    /// Roles assigned to this group.
    /// </summary>
    public ICollection<GroupRole> GroupRoles { get; set; } = new List<GroupRole>();
}
