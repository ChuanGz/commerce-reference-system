using IdentityService;
using IdentityService.Domain.Repositories;
using IdentityService.Application.Extensions;
using IdentityService.Infrastructure.Repositories;
using Platform.Core.Extensions;

var builder = WebApp.CreateWithDefaults<IdentityServiceEntry>(args);

builder.Services.AddIdentityAuthorizationPolicies();

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();
