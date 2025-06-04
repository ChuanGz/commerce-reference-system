namespace IdentityService.Application.Models;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<Guid> PermissionIds { get; set; } = [];
}
