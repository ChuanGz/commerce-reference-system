using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace OrderService.IntegrationTests;

public class AuthGateTests {
    [Fact]
    public async Task ProtectedEndpoint_Returns401_WhenNoToken() {
        await using var factory = new OrderServiceFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/orders");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task HealthEndpoint_Returns200_WithoutAuth() {
        await using var factory = new OrderServiceFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

internal sealed class OrderServiceFactory : WebApplicationFactory<Program> {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        Environment.SetEnvironmentVariable("Auth__Authority", "https://test.local");
        Environment.SetEnvironmentVariable("Auth__Audience", "test-api");

        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((_, config) => {
            config.AddInMemoryCollection(
                new Dictionary<string, string?> {
                    ["Auth:Authority"] = "https://test.local",
                    ["Auth:Audience"] = "test-api",
                }
            );
        });

        builder.ConfigureServices(services => {
            services.RemoveAll<IConfigureOptions<AuthenticationOptions>>();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.SchemeName,
                _ => { }
            );
        });
    }
}

internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
    public const string SchemeName = "Test";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
