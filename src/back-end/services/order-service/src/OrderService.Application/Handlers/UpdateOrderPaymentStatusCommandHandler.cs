using Microsoft.Extensions.Logging;
using OrderService.Application.Commands;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers {
    public class UpdateOrderPaymentStatusCommandHandler(
        IOrderRepository repo,
        ILogger<UpdateOrderPaymentStatusCommandHandler> logger
    ) : IRequestHandler<UpdateOrderPaymentStatusCommand, bool> {
        public async Task<bool> Handle(
            UpdateOrderPaymentStatusCommand command,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(command);

            logger.LogInformation(
                "Updating payment status for order: {OrderId} - Status: {PaymentStatus}",
                command.OrderId,
                command.PaymentStatus
            );

            try {
                var order = await repo.GetByIdAsync(command.OrderId, cancellationToken);
                if (order == null) {
                    logger.LogWarning("Order not found: {OrderId}", command.OrderId);
                    return false;
                }

                if (command.PaymentStatus == "Completed") {
                    order.Status = "Confirmed";
                }
                else if (command.PaymentStatus == "Failed") {
                    order.Status = "PaymentFailed";
                }

                await repo.UpdateAsync(order, cancellationToken);

                logger.LogInformation(
                    "Order payment status updated successfully: {OrderId} - New Status: {Status}",
                    command.OrderId,
                    order.Status
                );
                return true;
            }
            catch (Exception ex) {
                logger.LogError(
                    ex,
                    "Error updating payment status for order: {OrderId}",
                    command.OrderId
                );
                return false;
            }
        }
    }
}
