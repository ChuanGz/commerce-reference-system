namespace UserService.Application.Interfaces;

public interface IIdentityServiceClient
{
    Task<bool> ValidateUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserPermissions> GetUserPermissionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
}

public class UserPermissions
{
    public IEnumerable<string> Permissions { get; set; } = [];
}
