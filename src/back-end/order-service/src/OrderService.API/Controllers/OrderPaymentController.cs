using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderPaymentController(
    IMediator mediator
) : ControllerBase
{
    [HttpPut("{orderId:guid}/payment-status")]
    public async Task<IActionResult> UpdatePaymentStatus(
        Guid orderId,
        [FromBody] UpdatePaymentStatusRequest request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new UpdateOrderPaymentStatusCommand(orderId, request.PaymentStatus);
        var result = await mediator.Send(command, cancellationToken);

        return result ? Ok() : NotFound();
    }
}

public class UpdatePaymentStatusRequest
{
    public string PaymentStatus { get; set; } = string.Empty;
}
