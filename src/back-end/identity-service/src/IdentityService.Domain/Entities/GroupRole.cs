namespace IdentityService.Domain.Entities;

public class GroupRole
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
}
