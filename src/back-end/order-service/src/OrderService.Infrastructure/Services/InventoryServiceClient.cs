using System.Text.Json;
using Microsoft.Extensions.Logging;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services;

public class InventoryServiceClient(HttpClient httpClient, ILogger<InventoryServiceClient> logger)
    : IInventoryServiceClient
{
    public async Task<bool> CheckStockAvailabilityAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation(
                "Checking stock availability: {ProductId} - Quantity: {Quantity}",
                productId,
                quantity
            );

            var response = await httpClient.GetAsync(
                $"api/inventory/product/{productId}/availability?quantity={quantity}",
                cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonSerializer.Deserialize<AvailabilityResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                logger.LogInformation(
                    "Stock availability check: {ProductId} - Available: {IsAvailable}",
                    productId,
                    result?.IsAvailable ?? false
                );
                return result?.IsAvailable ?? false;
            }

            logger.LogWarning(
                "Stock availability check failed: {ProductId} - Status: {StatusCode}",
                productId,
                response.StatusCode
            );
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking stock availability: {ProductId}", productId);
            return false;
        }
    }

    public async Task<bool> ReserveStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation(
                "Reserving stock: {ProductId} - Quantity: {Quantity}",
                productId,
                quantity
            );

            var payload = new { productId, quantity };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(
                "api/inventory/reserve",
                content,
                cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation(
                    "Stock reserved successfully: {ProductId} - Quantity: {Quantity}",
                    productId,
                    quantity
                );
                return true;
            }

            logger.LogWarning(
                "Stock reservation failed: {ProductId} - Status: {StatusCode}",
                productId,
                response.StatusCode
            );
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reserving stock: {ProductId}", productId);
            return false;
        }
    }

    public async Task<bool> ReleaseStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation(
                "Releasing stock: {ProductId} - Quantity: {Quantity}",
                productId,
                quantity
            );

            var payload = new { productId, quantity };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(
                "api/inventory/release",
                content,
                cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation(
                    "Stock released successfully: {ProductId} - Quantity: {Quantity}",
                    productId,
                    quantity
                );
                return true;
            }

            logger.LogWarning(
                "Stock release failed: {ProductId} - Status: {StatusCode}",
                productId,
                response.StatusCode
            );
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error releasing stock: {ProductId}", productId);
            return false;
        }
    }

    public async Task<InventoryInfo?> GetInventoryAsync(
        Guid productId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation("Fetching inventory info: {ProductId}", productId);

            var response = await httpClient.GetAsync(
                $"api/inventory/product/{productId}",
                cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var inventory = JsonSerializer.Deserialize<InventoryInfo>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                logger.LogInformation(
                    "Inventory info retrieved successfully: {ProductId}",
                    productId
                );
                return inventory;
            }

            logger.LogWarning(
                "Inventory not found: {ProductId} - Status: {StatusCode}",
                productId,
                response.StatusCode
            );
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching inventory: {ProductId}", productId);
            return null;
        }
    }

    private class AvailabilityResponse
    {
        public bool IsAvailable { get; set; }
    }
}
