using IdentityService.Application.Commands;
using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authorization;

[ApiController]
[Route("api/usergroups")]
[Authorize]
public class UserGroupController(IMediator mediator) : ControllerBase
{
    [Authorize(Policy = "CanViewGroup")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetAllUserGroupsQuery(), cancellationToken);
        return Ok(result);
    }

    [Authorize(Policy = "CanEditGroup")]
    [HttpPost]
    public async Task<IActionResult> AssignUserToGroup([FromBody] UserGroupDto dto, CancellationToken cancellationToken = default)
    {
        var success = await mediator.Send(new AssignUserToGroupCommand(dto.UserId, dto.GroupId), cancellationToken);
        return success ? CreatedAtAction(nameof(GetAll), null) : Conflict("User is already in the group.");
    }

    [Authorize(Policy = "CanApproveGroup")]
    [HttpPut("approve")]
    public async Task<IActionResult> Approve([FromBody] UserGroupDto dto, CancellationToken cancellationToken = default)
    {
        var success = await mediator.Send(new ApproveUserGroupCommand(dto.UserId, dto.GroupId), cancellationToken);
        return success ? Ok("Approved.") : NotFound("Assignment not found.");
    }

    [Authorize(Policy = "CanDeleteGroup")]
    [HttpDelete]
    public async Task<IActionResult> Remove([FromBody] UserGroupDto dto, CancellationToken cancellationToken = default)
    {
        var success = await mediator.Send(new RemoveUserGroupCommand(dto.UserId, dto.GroupId), cancellationToken);
        return success ? NoContent() : NotFound("Assignment not found.");
    }
}
