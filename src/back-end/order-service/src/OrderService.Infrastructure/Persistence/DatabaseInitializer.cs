using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OrderService.Infrastructure.Persistence;

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
            var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            if (isDevelopment)
            {
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Database ensured for Order Service");
            }
            else
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrated for Order Service");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while initializing the database for Order Service"
            );
            throw;
        }
    }
}
