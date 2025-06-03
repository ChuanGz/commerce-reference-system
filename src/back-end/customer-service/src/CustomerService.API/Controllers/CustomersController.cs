using Microsoft.AspNetCore.Mvc;
using CustomerService.Application.Commands;
using CustomerService.Application.Queries;

namespace CustomerService.API.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        Ok(await mediator.Send(new GetAllCustomersQuery(), cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        var customer = await mediator.Send(new GetCustomerByUserIdQuery(userId), cancellationToken);

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var customerId = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = customerId }, command);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var updatedCommand = new UpdateCustomerCommand(id, command.FirstName, command.LastName, command.Phone, command.Address);

        var result = await mediator.Send(updatedCommand, cancellationToken);

        if (!result.Equals(Unit.Value))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new DeleteCustomerCommand(id), cancellationToken);

        if (!result.Equals(Unit.Value))
            return NotFound();

        return NoContent();
    }
}
