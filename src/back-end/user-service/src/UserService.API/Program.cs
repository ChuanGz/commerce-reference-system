using UserService.API.Middlewares;
using UserService.Application.Handlers;
using UserService.Application.Validators;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

var isDevelopmentEnv =
    app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName.Equals("Local", StringComparison.OrdinalIgnoreCase)
    || app.Environment.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase);

if (isDevelopmentEnv)
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
    DatabaseInitializer.InitializeAsync(app.Services, logger, isDevelopmentEnv)
        .ContinueWith(task =>
        {
            if (task.Exception != null)
                logger.LogError(task.Exception, "Database initialization failed");
        });
});

await app.RunAsync();
