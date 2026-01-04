using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CustomerService.Infrastructure.Persistence {
    public static class DatabaseInitializer {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider,
            ILogger logger,
            bool isDevelopment
        ) {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();

            try {
                if (isDevelopment) {
                    await context.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database ensured created for Customer Service");
                }
                else {
                    logger.LogInformation(
                        "Skipping automatic migrations for Customer Service in non-development environments"
                    );
                    return;
                }
            }
            catch (Exception ex) {
                logger.LogError(
                    ex,
                    "An error occurred while initializing the database for Customer Service"
                );
                throw;
            }
        }
    }
}
