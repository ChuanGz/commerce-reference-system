namespace IdentityService.Domain.Entities {
    public class User {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public ICollection<UserGroup> UserGroups { get; set; } = [];
    }
}
