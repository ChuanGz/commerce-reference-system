using IdentityService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(
        IServiceProvider serviceProvider,
        ILogger logger,
        bool isDevelopment
    )
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        if (isDevelopment)
        {
            logger.LogInformation("Development environment detected. Recreating database...");
            await context.Database.EnsureDeletedAsync();
            logger.LogInformation("Database deleted successfully");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration completed successfully");
        }
        else
        {
            logger.LogInformation("Production environment. Running migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration completed successfully");
        }

        if (context.Users.Any())
        {
            logger.LogInformation("Database already seeded");
            return;
        }

        var permissions = new[]
        {
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_VIEW_GROUP",
                Description = "View group information",
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_APPROVE_GROUP",
                Description = "Approve group requests",
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_EDIT_GROUP",
                Description = "Edit group details",
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Key = "CAN_DELETE_GROUP",
                Description = "Delete groups",
            },
        };

        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            RolePermissions = permissions
                .Select(p => new RolePermission { Permission = p })
                .ToList(),
        };

        var managerRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Manager",
            RolePermissions = permissions
                .Where(p => p.Key != "CAN_DELETE_GROUP")
                .Select(p => new RolePermission { Permission = p })
                .ToList(),
        };

        var employeeRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Employee",
            RolePermissions = permissions
                .Where(p => p.Key == "CAN_VIEW_GROUP")
                .Select(p => new RolePermission { Permission = p })
                .ToList(),
        };

        var auditorRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Auditor",
            RolePermissions = permissions
                .Where(p => p.Key == "CAN_VIEW_GROUP")
                .Select(p => new RolePermission { Permission = p })
                .ToList(),
        };

        var groups = new[]
        {
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "SystemAdmin",
                GroupRoles = [new GroupRole { Role = adminRole }],
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "BOD",
                GroupRoles = [new GroupRole { Role = managerRole }],
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "Customer",
                GroupRoles = [new GroupRole { Role = employeeRole }],
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "Employee",
                GroupRoles = [new GroupRole { Role = employeeRole }],
            },
            new Group
            {
                Id = Guid.NewGuid(),
                Name = "Auditor",
                GroupRoles = [new GroupRole { Role = auditorRole }],
            },
        };

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
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Abcd@@1234"),
                        UserGroups = [new UserGroup { Group = group }],
                    }
                );
            }
        }

        context.Permissions.AddRange(permissions);
        context.Roles.AddRange(adminRole, managerRole, employeeRole, auditorRole);
        context.Groups.AddRange(groups);
        context.Users.AddRange(users);

        await context.SaveChangesAsync();

        logger.LogInformation("Database seeded successfully");
    }
}
