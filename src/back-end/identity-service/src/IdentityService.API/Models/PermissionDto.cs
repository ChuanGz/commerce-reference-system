namespace IdentityService.API.Models;

public class PermissionDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
}
