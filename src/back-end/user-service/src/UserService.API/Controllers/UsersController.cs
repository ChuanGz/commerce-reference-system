using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands;
using UserService.Application.Queries;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetAllUsersQuery(), cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = userId }, command);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = new UpdateUserCommand(id, command.Name, command.Email);

        var result = await _mediator.Send(updatedCommand, cancellationToken);
        if (result.Equals(Unit.Value))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        if (!result.Equals(Unit.Value))
            return NotFound();

        return NoContent();
    }
}