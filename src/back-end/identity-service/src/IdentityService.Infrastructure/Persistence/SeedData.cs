using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Persistence;

/// <summary>
/// Seeds users, groups, roles, and permissions for realistic test scenarios.
/// </summary>
public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        await context.Database.MigrateAsync();

        if (context.Users.Any())
            return; // Already seeded

        // --- Permissions ---
        var permissions = new[]
        {
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_VIEW_GROUP",
                Description = "View group information"
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_APPROVE_GROUP",
                Description = "Approve group requests"
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_EDIT_GROUP",
                Description = "Edit group details"
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_DELETE_GROUP",
                Description = "Delete groups"
            },
        };

        // --- Roles ---
        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            RolePermissions = permissions
                .Select(p => new RolePermission { Permission = p })
                .ToList()
        };

        var managerRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Manager",
            RolePermissions = permissions
                .Where(p => p.Key != "CAN_DELETE_GROUP")
                .Select(p => new RolePermission { Permission = p })
                .ToList()
        };

        var employeeRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Employee",
            RolePermissions = permissions
                .Where(p => p.Key == "CAN_VIEW_GROUP")
                .Select(p => new RolePermission { Permission = p })
                .ToList()
        };

        var auditorRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Auditor",
            RolePermissions = permissions
                .Where(p => p.Key == "CAN_VIEW_GROUP")
                .Select(p => new RolePermission { Permission = p })
                .ToList()
        };

        // --- Groups ---
        var groups = new[]
        {
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "SystemAdmin",
                GroupRoles = new List<GroupRole> { new GroupRole { Role = adminRole } }
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "BOD",
                GroupRoles = new List<GroupRole> { new GroupRole { Role = managerRole } }
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "Customer",
                GroupRoles = new List<GroupRole> { new GroupRole { Role = employeeRole } }
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "Employee",
                GroupRoles = new List<GroupRole> { new GroupRole { Role = employeeRole } }
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "Auditor",
                GroupRoles = new List<GroupRole> { new GroupRole { Role = auditorRole } }
            },
        };

        // --- Users ---
        var users = new List<User>();

        foreach (var group in groups)
        {
            int userCount = group.Name == "Employee" ? 20 : 5;

            for (int i = 1; i <= userCount; i++)
            {
                users.Add(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = $"{group.Name.ToLower()}_{i}",
                        PasswordHash = "password", // Replace with hashed value
                        UserGroups = new List<UserGroup> { new UserGroup { Group = group } }
                    }
                );
            }
        }

        // Save everything to DB
        context.Permissions.AddRange(permissions);
        context.Roles.AddRange(adminRole, managerRole, employeeRole, auditorRole);
        context.Groups.AddRange(groups);
        context.Users.AddRange(users);

        await context.SaveChangesAsync();
    }
}
