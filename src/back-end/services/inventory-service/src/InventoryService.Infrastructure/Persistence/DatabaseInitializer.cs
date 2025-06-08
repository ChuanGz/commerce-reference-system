using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InventoryService.Infrastructure.Persistence
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider,
            ILogger logger,
            bool isDevelopment
        )
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

                if (isDevelopment)
                {
                    await context.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database ensured for Inventory Service");
                }
                else
                {
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Database migrated for Inventory Service");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred while initializing the database for Inventory Service"
                );
                throw;
            }
        }
    }
}
