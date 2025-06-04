using InventoryService.Application.Commands;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllInventoryQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetInventoryByIdQuery(id), cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProductId(
        Guid productId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _mediator.Send(
            new GetInventoryByProductIdQuery(productId),
            cancellationToken
        );
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateInventoryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateInventoryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(command);


        if (id != command.Id)
            return BadRequest(ErrorMessages.IdMismatch);

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteInventoryCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve(
        [FromBody] ReserveInventoryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("release")]
    public async Task<IActionResult> ReleaseReserved(
        [FromBody] ReleaseReservedInventoryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}
