using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Platform.Core.Extensions;

public static class WebApp {
    /// <summary>
    /// Creates a WebApplicationBuilder with preconfigured logging, configuration,
    /// Swagger, health checks, MediatR, FluentValidation, and Entity Framework Core support.
    /// Automatically infers service name and DbContext from the provided entry marker type.
    /// </summary>
    /// <typeparam name="TEntry">Marker type located in the target service's root namespace and assembly.</typeparam>
    public static WebApplicationBuilder CreateWithDefaults<TEntry>(string[] args)
        where TEntry : class {
        var builder = WebApplication.CreateBuilder(args);

        builder.UseDefaultLogging();

        ConfigureConfiguration(builder);

        var assembly = typeof(TEntry).Assembly;
        var serviceName = GetServiceName(typeof(TEntry));

        ConfigureAuth(builder);
        ConfigureApiDefaults(builder);
        ConfigureSwagger(builder, serviceName);
        ConfigureDbContext(builder, assembly);
        ConfigurePlatformServices(builder, assembly);

        return builder;
    }

    /// <summary>
    /// Apply default middleware pipeline and database initialization logic.
    /// </summary>
    public static void UseAppDefaults(this WebApplication app) {
        ArgumentNullException.ThrowIfNull(app);
        app.UsePlatformExceptionHandling();

        if (IsLocalOrDev(app.Environment)) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health").AllowAnonymous();

        InitDbFromRegisteredContext(app);
    }

    /// <summary>
    /// Check if the current environment is local or development.
    /// </summary>
    /// <param name="env"></param>
    /// <returns></returns>
    private static bool IsLocalOrDev(IHostEnvironment env) {
        return env.IsDevelopment()
            || env.EnvironmentName.Equals("Local", StringComparison.OrdinalIgnoreCase)
            || env.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase);
    }

    private static void ConfigureConfiguration(WebApplicationBuilder builder) {
        builder
            .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: true,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();
    }

    private static string GetServiceName(Type entryType) {
        var namespaceName = entryType.Namespace;
        return namespaceName?.Split('.').FirstOrDefault() ?? "API";
    }

    private static void ConfigureAuth(WebApplicationBuilder builder) {
        var authAuthority = builder.Configuration["Auth:Authority"];
        var authAudience = builder.Configuration["Auth:Audience"];

        if (string.IsNullOrWhiteSpace(authAuthority) || string.IsNullOrWhiteSpace(authAudience)) {
            throw new InvalidOperationException(
                "Missing auth configuration. Set Auth:Authority and Auth:Audience."
            );
        }

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.Authority = authAuthority;
                options.Audience = authAudience;
                options.RequireHttpsMetadata = true;
            });

        builder.Services.AddAuthorization(options => {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
    }

    private static void ConfigureApiDefaults(WebApplicationBuilder builder) {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHealthChecks();
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder, string serviceName) {
        builder.Services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{serviceName} API", Version = "v1" });
        });
    }

    private static void ConfigureDbContext(WebApplicationBuilder builder, Assembly assembly) {
        var dbContextType = assembly
            .GetTypes()
            .FirstOrDefault(t =>
                typeof(DbContext).IsAssignableFrom(t)
                && !t.IsAbstract
                && t.Name.EndsWith("DbContext")
            );

        if (dbContextType == null) {
            if (builder.Environment.IsEnvironment("Test")) {
                return;
            }

            throw new InvalidOperationException(
                "No concrete DbContext type found in the service assembly."
            );
        }

        var efMethods = typeof(EntityFrameworkServiceCollectionExtensions).GetMethods(
            BindingFlags.Public | BindingFlags.Static
        );

        var addDbContextMethod = efMethods.FirstOrDefault(m =>
            m.Name == "AddDbContext"
            && m.IsGenericMethodDefinition
            && m.GetGenericArguments().Length == 1
            && m.GetParameters().Length == 2
            && m.GetParameters()[1].ParameterType.Name.Contains("Action")
        );

        if (addDbContextMethod == null) {
            throw new InvalidOperationException("Unable to locate AddDbContext<TContext> method.");
        }

        var addDbContextGenericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);

        var dbContextOptionsDelegate = new Action<DbContextOptionsBuilder>(options => {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString)) {
                throw new InvalidOperationException(
                    "Missing 'DefaultConnection' in configuration."
                );
            }

            options.UseSqlServer(connectionString);
        });

        addDbContextGenericMethod.Invoke(null, [builder.Services, dbContextOptionsDelegate]);
    }

    private static void ConfigurePlatformServices(WebApplicationBuilder builder, Assembly assembly) {
        builder
            .Services.AddPlatformMediatR(assembly)
            .AddValidatorsFromAssembly(assembly)
            .AddPlatformValidation();
    }

    /// <summary>
    /// Attempt to resolve registered DbContext type and run the initializer.
    /// </summary>
    private static void InitDbFromRegisteredContext(WebApplication app) {
        // Try resolve the registered DbContext
        var dbContext = app.Services.GetServices<DbContext>().FirstOrDefault();
        if (dbContext is null) {
            if (app.Environment.IsEnvironment("Test")) {
                return;
            }

            throw new InvalidOperationException("No DbContext found to initialize.");
        }

        var dbContextType = dbContext.GetType();
        var dbContextName = dbContextType.Name;

        // Infer DatabaseInitializer type from naming convention
        var initializerTypeName = dbContextName.Replace("DbContext", "DatabaseInitializer");

        var initializerType = dbContextType
            .Assembly.GetTypes()
            .FirstOrDefault(t => t.IsClass && !t.IsAbstract && t.Name == initializerTypeName);

        if (initializerType == null)
            throw new InvalidOperationException(
                $"No initializer class named '{initializerTypeName}' found for {dbContextName}."
            );

        var loggerType = typeof(ILogger<>).MakeGenericType(dbContextType);
        var logger = app.Services.GetRequiredService(loggerType);

        var initMethod = initializerType.GetMethod(
            "InitializeAsync",
            BindingFlags.Public | BindingFlags.Static
        );
        if (initMethod == null)
            throw new InvalidOperationException(
                $"Method 'InitializeAsync' not found in '{initializerTypeName}'."
            );

        // Invoke the method dynamically
        var task = (Task?)
            initMethod.Invoke(
                null,
                new object[] { app.Services, logger, app.Environment.IsDevelopment() }
            );

        task?.ContinueWith(t => {
            if (t.Exception != null) {
                var log = app.Services.GetRequiredService<ILogger<WebApplication>>();
                log.LogError(t.Exception, "Database initialization failed.");
            }
        });
    }
}
