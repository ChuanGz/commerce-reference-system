using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Platform.Core.Logging;

namespace Platform.Core.Extensions;

public static class MediatRExtension {
    public static IServiceCollection AddPlatformMediatR(
        this IServiceCollection services,
        Assembly handlerAssembly
    ) {
        services.AddMediatR(handlerAssembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}
