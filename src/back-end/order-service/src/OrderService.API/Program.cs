using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderService.API.Middlewares;
using OrderService.Application.Behaviors;
using OrderService.Application.Handlers;
using OrderService.Application.Validators;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Service API", Version = "v1" });
});
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerServiceClient, CustomerServiceClient>();
builder.Services.AddScoped<IProductServiceClient, ProductServiceClient>();
builder.Services.AddScoped<IInventoryServiceClient, InventoryServiceClient>();

builder.Services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:CustomerService:BaseUrl"] ?? "http://localhost:5050"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:ProductService:BaseUrl"] ?? "http://localhost:5060"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IInventoryServiceClient, InventoryServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:InventoryService:BaseUrl"] ?? "http://localhost:5040"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

var isDev =
    app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName.Equals("Local", StringComparison.OrdinalIgnoreCase)
    || app.Environment.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase);

if (isDev)
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
        .InitializeAsync(app.Services, logger, isDev)
        .ContinueWith(task =>
        {
            if (task.Exception != null)
                logger.LogError(task.Exception, "Database initialization failed");
        });
});

await app.RunAsync();
