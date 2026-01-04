using IdentityService.Application.Constants;
using Microsoft.Extensions.DependencyInjection;
using Platform.Core.Extensions;

namespace IdentityService.Application.Extensions;

public static class AuthorizationPolicyExtensions {
    public static IServiceCollection AddIdentityAuthorizationPolicies(
        this IServiceCollection services
    ) {
        services.AddAuthorization(options => {
            foreach (var policy in AuthorizationPolicies.Map) {
                options.AddPolicy(policy.Key, builder => builder.RequireScope(policy.Value));
            }
        });

        return services;
    }
}
