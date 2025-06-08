using PaymentService;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Repositories;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.Services;
using Platform.Core.Extensions;

var builder = WebApp.CreateWithDefaults<PaymentServiceEntry>(args);

var app = builder.Build();

builder
    .Services.AddScoped<IPaymentRepository, PaymentRepository>()
    .AddScoped<ICustomerServiceClient, CustomerServiceClient>()
    .AddScoped<IOrderServiceClient, OrderServiceClient>();

builder.Services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:CustomerService:BaseUrl"] ?? "http://localhost:5050"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IOrderServiceClient, OrderServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:OrderService:BaseUrl"] ?? "http://localhost:5080"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
});

app.UseAppDefaults();

await app.RunAsync();
