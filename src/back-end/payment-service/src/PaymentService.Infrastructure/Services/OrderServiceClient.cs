using System.Text.Json;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Interfaces;

namespace PaymentService.Infrastructure.Services;

public class OrderServiceClient(HttpClient httpClient, ILogger<OrderServiceClient> logger)
    : IOrderServiceClient
{
    public async Task<OrderInfo?> GetOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation("Fetching order info: {OrderId}", orderId);

            var response = await httpClient.GetAsync($"api/orders/{orderId}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var order = JsonSerializer.Deserialize<OrderInfo>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                logger.LogInformation("Order info retrieved successfully: {OrderId}", orderId);
                return order;
            }

            logger.LogWarning(
                "Order not found: {OrderId} - Status: {StatusCode}",
                orderId,
                response.StatusCode
            );
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching order: {OrderId}", orderId);
            return null;
        }
    }

    public async Task<bool> ValidateOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation("Validating order: {OrderId}", orderId);

            var order = await GetOrderAsync(orderId, cancellationToken);
            var isValid = order != null && order.Status != "Cancelled";

            logger.LogInformation(
                "Order validation result: {OrderId} - {IsValid}",
                orderId,
                isValid
            );
            return isValid;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating order: {OrderId}", orderId);
            return false;
        }
    }

    public async Task<bool> UpdateOrderPaymentStatusAsync(
        Guid orderId,
        string paymentStatus,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogInformation(
                "Updating order payment status: {OrderId} - {PaymentStatus}",
                orderId,
                paymentStatus
            );

            var payload = new { paymentStatus };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PutAsync(
                $"api/orders/{orderId}/payment-status",
                content,
                cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation(
                    "Order payment status updated successfully: {OrderId}",
                    orderId
                );
                return true;
            }

            logger.LogWarning(
                "Failed to update order payment status: {OrderId} - Status: {StatusCode}",
                orderId,
                response.StatusCode
            );
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating order payment status: {OrderId}", orderId);
            return false;
        }
    }
}
