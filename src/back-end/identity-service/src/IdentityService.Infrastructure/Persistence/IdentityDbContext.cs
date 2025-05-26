using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Persistence;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{

    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<GroupRole> GroupRoles => Set<GroupRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });

        modelBuilder
            .Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId);

        modelBuilder
            .Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId);
        modelBuilder.Entity<GroupRole>().HasKey(gr => new { gr.GroupId, gr.RoleId });

        modelBuilder
            .Entity<GroupRole>()
            .HasOne(gr => gr.Group)
            .WithMany(g => g.GroupRoles)
            .HasForeignKey(gr => gr.GroupId);

        modelBuilder
            .Entity<GroupRole>()
            .HasOne(gr => gr.Role)
            .WithMany(r => r.GroupRoles)
            .HasForeignKey(gr => gr.RoleId);
        modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder
            .Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder
            .Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);
    }
}
