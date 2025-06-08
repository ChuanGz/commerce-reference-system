using Platform.Core.Extensions;
using UserService.API;
using UserService.Infrastructure.Repositories;

var builder = WebApp.CreateWithDefaults<UserServiceEntry>(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();
