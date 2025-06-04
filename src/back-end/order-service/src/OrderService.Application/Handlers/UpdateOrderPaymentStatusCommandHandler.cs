using Microsoft.Extensions.Logging;
using OrderService.Application.Commands;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class UpdateOrderPaymentStatusCommandHandler(
    IOrderRepository orderRepository,
    ILogger<UpdateOrderPaymentStatusCommandHandler> logger
) : IRequestHandler<UpdateOrderPaymentStatusCommand, bool>
{
    public async Task<bool> Handle(
        UpdateOrderPaymentStatusCommand request,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation(
            "Updating payment status for order: {OrderId} - Status: {PaymentStatus}",
            request.OrderId,
            request.PaymentStatus
        );

        try
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                logger.LogWarning("Order not found: {OrderId}", request.OrderId);
                return false;
            }

            if (request.PaymentStatus == "Completed")
            {
                order.Status = "Confirmed";
            }
            else if (request.PaymentStatus == "Failed")
            {
                order.Status = "PaymentFailed";
            }

            await orderRepository.UpdateAsync(order, cancellationToken);

            logger.LogInformation(
                "Order payment status updated successfully: {OrderId} - New Status: {Status}",
                request.OrderId,
                order.Status
            );
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error updating payment status for order: {OrderId}",
                request.OrderId
            );
            return false;
        }
    }
}
