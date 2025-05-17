namespace IdentityService.Domain.Entities;

/// <summary>
/// Join table: Group <-> Role (many-to-many).
/// </summary>
public class GroupRole
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
}
