using CustomerService.Application.Handlers;
using CustomerService.Domain.Repositories;
using CustomerService.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Platform.Core.Extensions;

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
builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Service API", Version = "v1" });
});

builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder
    .Services.AddScoped<ICustomerRepository, CustomerRepository>()
    .AddPlatformMediatR(typeof(CreateCustomerCommandHandler).Assembly)
    .AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>()
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
