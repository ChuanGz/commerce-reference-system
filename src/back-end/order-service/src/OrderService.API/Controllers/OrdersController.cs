using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using OrderService.Application.Queries;
using OrderService.Domain.Constants;


namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllOrdersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(
        Guid customerId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(
            new GetOrdersByCustomerIdQuery(customerId),
            cancellationToken
        );
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(
        string status,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(new GetOrdersByStatusQuery(status), cancellationToken);
        return Ok(result);
    }

    [HttpGet("date-range")]
    public async Task<IActionResult> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(
            new GetOrdersByDateRangeQuery(startDate, endDate),
            cancellationToken
        );
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateOrderStatusCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (id != command.Id)
            return BadRequest(ErrorMessages.IdMismatch);

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteOrderCommand(id), cancellationToken);
        return NoContent();
    }
}
