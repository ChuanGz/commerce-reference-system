using Platform.Core.Extensions;
using ProductService;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Repositories;

var builder = WebApp.CreateWithDefaults<ProductServiceEntry>(args);
builder.Services.AddServiceAuthorizationPolicies("ProductService");
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();
