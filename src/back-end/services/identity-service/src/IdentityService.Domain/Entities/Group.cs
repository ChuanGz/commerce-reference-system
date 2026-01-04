namespace IdentityService.Domain.Entities {
    public class Group {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<UserGroup> UserGroups { get; set; } = [];
        public ICollection<GroupRole> GroupRoles { get; set; } = [];
    }
}
