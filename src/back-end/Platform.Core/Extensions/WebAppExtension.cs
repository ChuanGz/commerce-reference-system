using System.Reflection;
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

        // Configure default logging
        builder.UseDefaultLogging();

        // Load configuration from environment-specific appsettings files and environment variables
        builder
            .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: true,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();

        // Determine service name and assembly
        var assembly = typeof(TEntry).Assembly;
        var namespaceName = typeof(TEntry).Namespace;
        var serviceName = namespaceName?.Split('.').FirstOrDefault() ?? "API";

        // Register common services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHealthChecks();

        // Configure Swagger with inferred service name
        builder.Services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{serviceName} API", Version = "v1" });
        });

        // Locate the DbContext implementation within the same assembly
        var dbContextType = assembly
            .GetTypes()
            .FirstOrDefault(t =>
                typeof(DbContext).IsAssignableFrom(t)
                && !t.IsAbstract
                && t.Name.EndsWith("DbContext")
            );

        if (dbContextType == null) {
            throw new InvalidOperationException(
                "No concrete DbContext type found in the service assembly."
            );
        }

        // Find the EF Core AddDbContext<TContext> method via reflection
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

        // Generate the generic method with the actual DbContext type
        var addDbContextGenericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);

        // Build the options delegate using the configured connection string
        var dbContextOptionsDelegate = new Action<DbContextOptionsBuilder>(options => {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString)) {
                throw new InvalidOperationException(
                    "Missing 'DefaultConnection' in configuration."
                );
            }

            options.UseSqlServer(connectionString);
        });

        // Invoke AddDbContext<TContext>(IServiceCollection, Action<DbContextOptionsBuilder>)
        addDbContextGenericMethod.Invoke(null, [builder.Services, dbContextOptionsDelegate]);

        // Register MediatR, FluentValidation, and validation pipeline behaviors
        builder
            .Services.AddPlatformMediatR(assembly)
            .AddValidatorsFromAssembly(assembly)
            .AddPlatformValidation();

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
        app.MapControllers();
        app.MapHealthChecks("/health");

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

    /// <summary>
    /// Attempt to resolve registered DbContext type and run the initializer.
    /// </summary>
    private static void InitDbFromRegisteredContext(WebApplication app) {
        // Try resolve the registered DbContext
        var dbContext = app.Services.GetServices<DbContext>().FirstOrDefault();
        if (dbContext is null)
            throw new InvalidOperationException("No DbContext found to initialize.");

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
