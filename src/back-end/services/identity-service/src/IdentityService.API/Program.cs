using IdentityService;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Repositories;
using Platform.Core.Extensions;

var builder = WebApp.CreateWithDefaults<IdentityServiceEntry>(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();
