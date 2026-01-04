using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Application.Handlers;
using OrderService.Application.Validators;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using Platform.Core.Extensions;
using Xunit;
using FluentValidation;

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
            services.AddDbContext<OrderDbContext>(options =>
                options.UseInMemoryDatabase("order-api-tests")
            );
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddPlatformMediatR(typeof(CreateOrderCommandHandler).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateOrderCommandValidator).Assembly);
            services.AddPlatformValidation();

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
        System.Text.Encodings.Web.UrlEncoder encoder
    ) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        var hasAuthHeader = Request.Headers.ContainsKey("Authorization");
        if (!hasAuthHeader)
            return Task.FromResult(AuthenticateResult.NoResult());

        var claims = new[] {
            new Claim("scp", "OrderService.read OrderService.write")
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
