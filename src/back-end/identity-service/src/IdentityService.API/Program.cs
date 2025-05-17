var builder = WebApplication.CreateBuilder(args);

// --- Add Controllers + Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Add EF Core with SQL Server ---
builder.Services.AddDbContext<IdentityDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
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

// --- Add Authorization Policies ---
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

var app = builder.Build();

// --- Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// --- Seed initial data (dev only) ---
await SeedData.InitializeAsync(app.Services);

await app.RunAsync();
