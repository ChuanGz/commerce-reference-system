namespace IdentityService.Application.Models;

public class UserGroupDto
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
}

public class UserGroupResponseDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = default!;
    public List<string> Groups { get; set; } = [];
}
