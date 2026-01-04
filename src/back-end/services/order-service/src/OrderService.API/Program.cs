using OrderService;
using Platform.Core.Extensions;

var builder = WebApp.CreateWithDefaults<OrderServiceEntry>(args);

builder.Services.AddServiceAuthorizationPolicies("OrderService");

builder.Services.AddScoped<ICustomerServiceClient, CustomerServiceClient>();
builder.Services.AddScoped<IProductServiceClient, ProductServiceClient>();
builder.Services.AddScoped<IInventoryServiceClient, InventoryServiceClient>();

builder.Services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client => {
    client.BaseAddress = new Uri(
        builder.Configuration["Services:CustomerService:BaseUrl"] ?? "http://localhost:5050"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(client => {
    client.BaseAddress = new Uri(
        builder.Configuration["Services:ProductService:BaseUrl"] ?? "http://localhost:5060"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IInventoryServiceClient, InventoryServiceClient>(client => {
    client.BaseAddress = new Uri(
        builder.Configuration["Services:InventoryService:BaseUrl"] ?? "http://localhost:5040"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();

public partial class Program { }
