using InventoryService;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Repositories;
using Platform.Core.Extensions;

var builder = WebApp.CreateWithDefaults<InventoryServiceEntry>(args);

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();
