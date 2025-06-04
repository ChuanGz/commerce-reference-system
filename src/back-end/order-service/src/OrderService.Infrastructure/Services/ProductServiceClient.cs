using System.Text.Json;
using Microsoft.Extensions.Logging;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services;

public class ProductServiceClient(HttpClient httpClient, ILogger<ProductServiceClient> logger)
    : IProductServiceClient
{
    public async Task<ProductInfo?> GetProductAsync(
        Guid productId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation("Fetching product info: {ProductId}", productId);

            var response = await httpClient.GetAsync(
                $"api/products/{productId}",
                cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var product = JsonSerializer.Deserialize<ProductInfo>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                logger.LogInformation(
                    "Product info retrieved successfully: {ProductId}",
                    productId
                );
                return product;
            }

            logger.LogWarning(
                "Product not found: {ProductId} - Status: {StatusCode}",
                productId,
                response.StatusCode
            );
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching product: {ProductId}", productId);
            return null;
        }
    }

    public async Task<bool> ValidateProductAsync(
        Guid productId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation("Validating product: {ProductId}", productId);

            var product = await GetProductAsync(productId, cancellationToken);
            var isValid = product != null && product.IsActive;

            logger.LogInformation(
                "Product validation result: {ProductId} - {IsValid}",
                productId,
                isValid
            );
            return isValid;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating product: {ProductId}", productId);
            return false;
        }
    }

    public async Task<IEnumerable<ProductInfo>> GetProductsAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation("Fetching multiple products: {ProductCount}", productIds.Count());

            var tasks = productIds.Select(id => GetProductAsync(id, cancellationToken));
            var results = await Task.WhenAll(tasks);

            var products = results.Where(p => p != null).Cast<ProductInfo>().ToList();
            logger.LogInformation("Retrieved {ProductCount} products successfully", products.Count);

            return products;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching multiple products");
            return [];
        }
    }
}
