using System.Text.Json;
using PaymentService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace PaymentService.Infrastructure.Services;

public class CustomerServiceClient(HttpClient httpClient, ILogger<CustomerServiceClient> logger) : ICustomerServiceClient
{
    public async Task<CustomerInfo?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Fetching customer info: {CustomerId}", customerId);
            
            var response = await httpClient.GetAsync($"api/customers/{customerId}", cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var customer = JsonSerializer.Deserialize<CustomerInfo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                logger.LogInformation("Customer info retrieved successfully: {CustomerId}", customerId);
                return customer;
            }
            
            logger.LogWarning("Customer not found: {CustomerId} - Status: {StatusCode}", customerId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching customer: {CustomerId}", customerId);
            return null;
        }
    }

    public async Task<bool> ValidateCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Validating customer: {CustomerId}", customerId);
            
            var customer = await GetCustomerAsync(customerId, cancellationToken);
            var isValid = customer != null && customer.IsActive;
            
            logger.LogInformation("Customer validation result: {CustomerId} - {IsValid}", customerId, isValid);
            return isValid;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating customer: {CustomerId}", customerId);
            return false;
        }
    }
}
