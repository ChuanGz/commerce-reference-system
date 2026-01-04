using CustomerService;
using CustomerService.Domain.Repositories;
using CustomerService.Infrastructure.Repositories;
using Platform.Core.Extensions;

var builder = WebApp.CreateWithDefaults<CustomerServiceEntry>(args);

builder.Services.AddServiceAuthorizationPolicies("CustomerService");

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

app.UseAppDefaults();

await app.RunAsync();
