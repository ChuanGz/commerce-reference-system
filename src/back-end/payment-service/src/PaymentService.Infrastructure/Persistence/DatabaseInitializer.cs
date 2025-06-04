using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PaymentService.Infrastructure.Persistence;

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
            var context = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

            if (isDevelopment)
            {
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Database ensured for Payment Service");
            }
            else
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrated for Payment Service");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while initializing the database for Payment Service"
            );
            throw;
        }
    }
}
