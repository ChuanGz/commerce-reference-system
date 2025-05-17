using IdentityService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

/// <summary>
/// Handles authentication requests such as login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Login endpoint — validates credentials and returns a JWT token.
    /// </summary>
    /// <param name="command">Login credentials (username, password)</param>
    /// <param name="cancellationToken">Bound to the HTTP request lifecycle</param>
    /// <returns>JWT token or 401 Unauthorized</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken
    )
    {
        var token = await _mediator.Send(command, cancellationToken);

        if (token is null)
            return Unauthorized("Invalid credentials");

        return Ok(new { token });
    }
}
