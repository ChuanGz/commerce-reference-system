using Microsoft.AspNetCore.Builder;

namespace Platform.Core.Extensions;

public static class LoggingExtension
{
    public static void UseDefaultLogging(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
    }
}
