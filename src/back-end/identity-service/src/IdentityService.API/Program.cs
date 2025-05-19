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


Console.WriteLine("== Loaded MS SQL ServerConnection ==");
Console.WriteLine(
    $"\r\n    DefaultConnection: {builder.Configuration.GetConnectionString("DefaultConnection")}\r\n"
);


// EF Core DbContext
builder.Services.AddDbContext<IdentityDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sql => sql.UseRelationalNulls())

);

// --- Add JWT Authentication ---
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
            )
        };
    });

// Authorization + Policies
builder.Services.AddAuthorization(options =>
{
    // Group access policies
    options.AddPolicy(
        "CanViewGroup",
        policy => policy.RequireClaim("permission", "CAN_VIEW_GROUP")
    );
    options.AddPolicy(
        "CanApproveGroup",
        policy => policy.RequireClaim("permission", "CAN_APPROVE_GROUP")
    );
    options.AddPolicy(
        "CanEditGroup",
        policy => policy.RequireClaim("permission", "CAN_EDIT_GROUP")
    );
    options.AddPolicy(
        "CanDeleteGroup",
        policy => policy.RequireClaim("permission", "CAN_DELETE_GROUP")
    );

    // Role & permission management (for RoleController / PermissionController)
    options.AddPolicy(
        "CanViewPermission",
        policy => policy.RequireClaim("permission", "CAN_VIEW_PERMISSION")
    );
    options.AddPolicy("CanViewRole", policy => policy.RequireClaim("permission", "CAN_VIEW_ROLE"));
    options.AddPolicy("CanEditRole", policy => policy.RequireClaim("permission", "CAN_EDIT_ROLE"));
    options.AddPolicy(
        "CanDeleteRole",
        policy => policy.RequireClaim("permission", "CAN_DELETE_ROLE")
    );
    options.AddPolicy(
        "CanAssignRolePermission",
        policy => policy.RequireClaim("permission", "CAN_ASSIGN_ROLE_PERMISSION")
    );
});

// DI for repository + MediatR

var app = builder.Build();
var isDevelopmentEnv =
    app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName == "Local"
    || app.Environment.EnvironmentName == "Docker";
// Swagger only in development
if (isDevelopmentEnv)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Map healthchecks
app.MapHealthChecks("/health");

try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

    // Auto migrate & seed data
    if (isDevelopmentEnv)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }

    Console.WriteLine("Connected to SQL Server successfully!");
    Console.WriteLine("== Seeded Data ==");

    var roles = await context.Roles.ToListAsync();
    var permissions = await context.Permissions.ToListAsync();
    var groups = await context.Groups.ToListAsync();

    static void PrintList<T>(
        string title,
        IEnumerable<T> items,
        Func<T, object[]> selector,
        string[] headers
    )
    {
        Console.WriteLine(new string('-', 60));
        Console.WriteLine(title);
        Console.WriteLine(string.Join(" | ", headers));
        Console.WriteLine(new string('-', 60));
        foreach (var item in items)
            Console.WriteLine(string.Join(" | ", selector(item)));
    }

    PrintList("Roles", roles, r => new object[] { r.Id, r.Name }, new[] { "ID", "Name" });
    PrintList(
        "Permissions",
        permissions,
        p => [p.Id, p.Description],
        ["ID", "Description"]
    );
    PrintList("Groups", groups, g => [g.Id, g.Name], ["ID", "Name"]);

    Console.WriteLine("==================");
}
catch (Exception ex)
{
    Console.WriteLine("Failed to connect to SQL Server:");
    Console.WriteLine(ex.Message);
}

await app.RunAsync();
