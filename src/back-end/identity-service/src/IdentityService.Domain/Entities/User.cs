namespace IdentityService.Domain.Entities;

/// <summary>
/// Represents an authenticated user in the system.
/// A user can belong to one or more groups.
/// </summary>
public class User
{
    public Guid Id { get; set; }

    /// <summary>
    /// Unique login name (username or email).
    /// </summary>
    public string Username { get; set; } = default!;

    /// <summary>
    /// Password hash (bcrypt or other). Can be extended with salt, 2FA flags, etc.
    /// </summary>
    public string PasswordHash { get; set; } = default!;

    /// <summary>
    /// Many-to-many: user memberships to groups.
    /// </summary>
    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
}
