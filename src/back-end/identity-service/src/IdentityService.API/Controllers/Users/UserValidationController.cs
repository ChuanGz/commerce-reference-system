using IdentityService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Users;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserValidationController(
    IMediator mediator
) : ControllerBase
{
    [HttpGet("{userId:guid}/validate")]
    public async Task<IActionResult> ValidateUser(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await mediator.Send(new GetUserByIdQuery(userId), cancellationToken);
        return user != null ? Ok(new { isValid = true }) : NotFound(new { isValid = false });
    }

    [HttpGet("{userId:guid}/permissions")]
    public async Task<IActionResult> GetUserPermissions(Guid userId, CancellationToken cancellationToken = default)
    {
        var permissions = await mediator.Send(new GetUserPermissionsQuery(userId), cancellationToken);
        return Ok(new { permissions });
    }
}
