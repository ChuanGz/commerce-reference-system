using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authorization;

[ApiController]
[Route("api/permissions")]
[Authorize(Policy = "CanViewPermission")]
public class PermissionsController(
    IMediator mediator
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PermissionDto>>> GetAll(
        CancellationToken cancellationToken = default
    )
    {
        var permissions = await mediator.Send(new GetAllPermissionsQuery(), cancellationToken);
        return Ok(permissions);
    }
}
