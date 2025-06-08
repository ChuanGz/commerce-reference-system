using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Platform.Core.Extensions;
using UserService.Application.Handlers;
using UserService.Application.Interfaces;
using UserService.Application.Validators;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.UseDefaultLogging();

builder
    .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service API", Version = "v1" });
});
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder
    .Services.AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IIdentityServiceClient, IdentityServiceClient>()
    .AddPlatformMediatR(typeof(CreateUserCommandHandler).Assembly)
    .AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>()
    .AddPlatformValidation();

builder.Services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:IdentityService:BaseUrl"] ?? "http://localhost:5070"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UsePlatformExceptionHandling();

if (
    app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName.Equals("Local", StringComparison.OrdinalIgnoreCase)
    || app.Environment.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase)
)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/health");

app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    DatabaseInitializer
        .InitializeAsync(app.Services, logger, app.Environment.IsDevelopment())
        .ContinueWith(task =>
        {
            if (task.Exception != null)
                logger.LogError(task.Exception, "Database initialization failed");
        });
});

await app.RunAsync();
