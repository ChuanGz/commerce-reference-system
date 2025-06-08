using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Platform.Core.Extensions;
using ProductService.Application.Handlers;
using ProductService.Application.Validators;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Persistence;
using ProductService.Infrastructure.Repositories;

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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Service API", Version = "v1" });
});
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder
    .Services.AddScoped<IProductRepository, ProductRepository>()
    .AddPlatformMediatR(typeof(CreateProductCommandHandler).Assembly)
    .AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>()
    .AddPlatformValidation();

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
