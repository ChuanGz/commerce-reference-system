using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Platform.Core.Validation;

namespace Platform.Core.Extensions;

public static class ValidationExtension
{
    public static IServiceCollection AddPlatformValidation(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
