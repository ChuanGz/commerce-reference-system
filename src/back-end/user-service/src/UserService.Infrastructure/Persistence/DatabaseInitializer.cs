using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserService.Infrastructure.Persistence
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services, ILogger logger, bool isDevelopmentEnv)
        {
            await using var scope = services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

            if (isDevelopmentEnv)
            {
                var dbExists = await context.Database.CanConnectAsync();
                if (dbExists)
                {
                    logger.LogInformation("Database exists. Deleting and recreating...");
                    await context.Database.EnsureDeletedAsync();
                }
                else
                {
                    logger.LogInformation("Database does not exist. Creating...");
                }
                await context.Database.MigrateAsync();
            }

            logger.LogInformation("Connected to SQL Server successfully!");
        }
    }
}
