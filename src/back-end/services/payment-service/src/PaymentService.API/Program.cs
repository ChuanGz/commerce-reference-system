using PaymentService;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Repositories;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.Services;
using Platform.Core.Extensions;
using Polly;
using Polly.Extensions.Http;

var builder = WebApp.CreateWithDefaults<PaymentServiceEntry>(args);

builder.Services.AddServiceAuthorizationPolicies("PaymentService");

builder
    .Services.AddScoped<IPaymentRepository, PaymentRepository>()
    .AddScoped<ICustomerServiceClient, CustomerServiceClient>()
    .AddScoped<IOrderServiceClient, OrderServiceClient>();

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

builder.Services.AddHttpClient<IOrderServiceClient, OrderServiceClient>(client => {
    client.BaseAddress = new Uri(
        builder.Configuration["Services:OrderService:BaseUrl"] ?? "http://localhost:5080"
    );
    client.Timeout = TimeSpan.FromSeconds(30);
})
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();
