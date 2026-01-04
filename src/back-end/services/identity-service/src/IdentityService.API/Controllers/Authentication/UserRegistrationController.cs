using IdentityService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authentication {
    [ApiController]
    [Route("api/register")]
    [AllowAnonymous]
    public class UserRegistrationController(IMediator mediator) : ControllerBase {
        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] UserRegistrationCommand command,
            CancellationToken cancellationToken = default
        ) {
            var result = await mediator.Send(command, cancellationToken);

            if (result is null)
                return Conflict("Username already taken.");

            return CreatedAtAction(nameof(Register), new { result.Id, result.Username });
        }
    }
}
