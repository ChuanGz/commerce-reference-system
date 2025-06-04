using System.Text.Json.Serialization;
using FluentValidation;
using IdentityService.Application.Constants;
using IdentityService.Application.Handlers;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Repositories;
using IdentityService.Infrastructure.Services;
using ISharedCommon.Middlewares;
using MediatR;
using Microsoft.OpenApi.Models;
using SharedCommon.Middlewares;
using SharedCommon.ValidationBehavior;
using UserService.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder
    .Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

builder
    .Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service API", Version = "v1" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT token only (no Bearer prefix)",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IUserGroupRepository, UserGroupRepository>();
builder.Services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
builder.Services.AddMediatR(typeof(AddRolePermissionCommandHandler).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            ),
        };
    });

builder.Services.AddAuthorization(options =>
{
    foreach (var (policyName, claim) in AuthorizationPolicies.Map)
    {
        options.AddPolicy(policyName, policy => policy.RequireClaim("permission", claim));
    }
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
app.UseAuthentication();
app.UseAuthorization();
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
