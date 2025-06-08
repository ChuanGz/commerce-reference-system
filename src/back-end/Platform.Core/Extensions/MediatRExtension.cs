using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Platform.Core.Extensions;

public static class MediatRExtension
{
    public static IServiceCollection AddPlatformMediatR(
        this IServiceCollection services,
        Assembly handlerAssembly
    )
    {
        services.AddMediatR(handlerAssembly);
        return services;
    }
}
