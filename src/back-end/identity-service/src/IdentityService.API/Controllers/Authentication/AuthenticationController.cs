using IdentityService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] AuthenticateUserCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var token = await mediator.Send(command, cancellationToken);

        if (token is null)
            return Unauthorized("Invalid credentials");

        return Ok(new { token });
    }
}
