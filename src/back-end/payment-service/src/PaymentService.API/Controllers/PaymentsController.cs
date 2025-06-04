using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Commands;
using PaymentService.Application.Queries;
using PaymentService.Domain.Constants;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllPaymentsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetPaymentByIdQuery(id), cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(
        Guid orderId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new GetPaymentByOrderIdQuery(orderId), cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(
        string status,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new GetPaymentsByStatusQuery(status), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdatePaymentStatusCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (id != command.Id)
            return BadRequest(ErrorMessages.IdMismatch);

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/process")]
    public async Task<IActionResult> Process(Guid id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new ProcessPaymentCommand(id), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeletePaymentCommand(id), cancellationToken);
        return NoContent();
    }
}
