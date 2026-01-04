using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Platform.Core.Extensions;

public static class AuthorizationPolicyExtensions {
    private const string ScopeClaimType = "scp";
    private const string AlternateScopeClaimType = "scope";

    public static IServiceCollection AddServiceAuthorizationPolicies(
        this IServiceCollection services,
        string serviceName
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceName);

        services.AddAuthorization(options => {
            options.AddPolicy($"{serviceName}.Read", policy =>
                policy.RequireScope($"{serviceName}.read")
            );
            options.AddPolicy($"{serviceName}.Write", policy =>
                policy.RequireScope($"{serviceName}.write")
            );
        });

        return services;
    }

    public static AuthorizationPolicyBuilder RequireScope(
        this AuthorizationPolicyBuilder builder,
        string requiredScope
    ) {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(requiredScope);

        return builder.RequireAssertion(context => {
            var scopes = context.User
                .FindAll(ScopeClaimType)
                .SelectMany(c => c.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Concat(
                    context.User.FindAll(AlternateScopeClaimType).SelectMany(c =>
                        c.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    )
                );

            return scopes.Contains(requiredScope, StringComparer.OrdinalIgnoreCase);
        });
    }
}
