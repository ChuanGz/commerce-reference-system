using IdentityService.API.Middlewares;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewGroup", policy => policy.RequireClaim("permission", "CAN_VIEW_GROUP"));
    options.AddPolicy("CanApproveGroup", policy => policy.RequireClaim("permission", "CAN_APPROVE_GROUP"));
    options.AddPolicy("CanEditGroup", policy => policy.RequireClaim("permission", "CAN_EDIT_GROUP"));
    options.AddPolicy("CanDeleteGroup", policy => policy.RequireClaim("permission", "CAN_DELETE_GROUP"));
    options.AddPolicy("CanViewPermission", policy => policy.RequireClaim("permission", "CAN_VIEW_PERMISSION"));
    options.AddPolicy("CanViewRole", policy => policy.RequireClaim("permission", "CAN_VIEW_ROLE"));
    options.AddPolicy("CanEditRole", policy => policy.RequireClaim("permission", "CAN_EDIT_ROLE"));
    options.AddPolicy("CanDeleteRole", policy => policy.RequireClaim("permission", "CAN_DELETE_ROLE"));
    options.AddPolicy("CanAssignRolePermission", policy => policy.RequireClaim("permission", "CAN_ASSIGN_ROLE_PERMISSION"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

var isDev = app.Environment.IsDevelopment()
    || app.Environment.EnvironmentName.Equals("Local", StringComparison.OrdinalIgnoreCase)
    || app.Environment.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase);

if (isDev)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    DatabaseInitializer.InitializeAsync(app.Services, logger, isDev)
        .ContinueWith(task =>
        {
            if (task.Exception != null)
                logger.LogError(task.Exception, "Database initialization failed");
        });
});

await app.RunAsync();
