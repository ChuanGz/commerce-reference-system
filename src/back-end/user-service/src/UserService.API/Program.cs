using UserService.Application.Handlers;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"ENVIRONMENT: {builder.Environment.EnvironmentName}");

// Load configuration from appsettings.json and environment variables
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

// Add MVC controllers
builder.Services.AddControllers();

// Swagger & healthchecks
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

Console.WriteLine("== Loaded DefaultConnection ==");
Console.WriteLine(
    $"\r\n    DefaultConnection: {builder.Configuration.GetConnectionString("DefaultConnection")}\r\n"
);

// EF Core DbContext
builder.Services.AddDbContext<UserDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// DI for repository + MediatR
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);

var app = builder.Build();

// Swagger only in development
if (
    app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName == "Local"
    || app.Environment.EnvironmentName == "Docker"
)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controller routes
app.MapControllers();

// Map healthchecks
app.MapHealthChecks("/health");

try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    // Auto migrate & seed data
    await context.Database.MigrateAsync();

    Console.WriteLine("Connected to SQL Server successfully!");
    Console.WriteLine("== Seeded Users ==");

    var users = await context.Users.ToListAsync();

    // Print header
    Console.WriteLine("{0,-38} | {1,-20} | {2}", "ID", "Name", "Email");
    Console.WriteLine(new string('-', 90));

    // Print rows
    foreach (var user in users)
    {
        Console.WriteLine("{0,-38} | {1,-20} | {2}", user.Id, user.Name, user.Email);
    }

    Console.WriteLine("==================");
}
catch (Exception ex)
{
    Console.WriteLine("Failed to connect to SQL Server:");
    Console.WriteLine(ex.Message);
}

await app.RunAsync();
