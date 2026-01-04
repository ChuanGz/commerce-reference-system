using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ProductService.Infrastructure.Persistence {
    public static class DatabaseInitializer {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider,
            ILogger logger,
            bool isDevelopment
        ) {
            try {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

                if (isDevelopment) {
                    await context.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database ensured for Product Service");
                }
                else {
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Database migrated for Product Service");
                }
            }
            catch (Exception ex) {
                logger.LogError(
                    ex,
                    "An error occurred while initializing the database for Product Service"
                );
                throw;
            }
        }
    }
}
