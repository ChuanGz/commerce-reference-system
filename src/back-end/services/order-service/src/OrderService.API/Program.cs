using OrderService;
using Platform.Core.Extensions;
using Polly;
using Polly.Extensions.Http;

var builder = WebApp.CreateWithDefaults<OrderServiceEntry>(args);

builder.Services.AddServiceAuthorizationPolicies("OrderService");

builder.Services.AddScoped<ICustomerServiceClient, CustomerServiceClient>();
builder.Services.AddScoped<IProductServiceClient, ProductServiceClient>();
builder.Services.AddScoped<IInventoryServiceClient, InventoryServiceClient>();

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200 * retry));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

builder.Services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client => {
    client.BaseAddress = new Uri(
        builder.Configuration["Services:CustomerService:BaseUrl"] ?? "http://localhost:5050"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
})
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(client => {
    client.BaseAddress = new Uri(
        builder.Configuration["Services:ProductService:BaseUrl"] ?? "http://localhost:5060"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
})
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddHttpClient<IInventoryServiceClient, InventoryServiceClient>(client => {
    client.BaseAddress = new Uri(
        builder.Configuration["Services:InventoryService:BaseUrl"] ?? "http://localhost:5040"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
})
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();

public partial class Program { }
