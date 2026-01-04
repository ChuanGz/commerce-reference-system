using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands;
using UserService.Application.Queries;

namespace UserService.API.Controllers {
    [ApiController]
    [Route("api/users")]
    public class UsersController(IMediator mediator) : ControllerBase {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
            Ok(await mediator.Send(new GetAllUsersQuery(), cancellationToken));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken = default
        ) {
            var user = await mediator.Send(new GetUserByIdQuery(id), cancellationToken);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserCommand command,
            CancellationToken cancellationToken = default
        ) {
            var userId = await mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = userId }, command);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateUserCommand command,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(command);
            var updatedCommand = new UpdateUserCommand(id, command.Name, command.Email);

            var result = await mediator.Send(updatedCommand, cancellationToken);

            if (!result.Equals(Unit.Value))
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken = default
        ) {
            var result = await mediator.Send(new DeleteUserCommand(id), cancellationToken);

            if (!result.Equals(Unit.Value))
                return NotFound();

            return NoContent();
        }
    }
}
