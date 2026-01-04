using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserService.Infrastructure.Persistence {
    public static class DatabaseInitializer {
        public static async Task InitializeAsync(
            IServiceProvider services,
            ILogger logger,
            bool isDevelopmentEnv
        ) {
            await using var scope = services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

            if (isDevelopmentEnv) {
                var dbExists = await context.Database.CanConnectAsync();
                if (dbExists) {
                    logger.LogInformation(
                        "Database exists. Deleting and recreating for development environment..."
                    );
                    await context.Database.EnsureDeletedAsync();
                    logger.LogInformation("Database deleted successfully");
                }
                else {
                    logger.LogInformation("Database does not exist. Creating new database...");
                }
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migration completed successfully");
            }

            logger.LogInformation("Connected to SQL Server successfully!");
        }
    }
}
