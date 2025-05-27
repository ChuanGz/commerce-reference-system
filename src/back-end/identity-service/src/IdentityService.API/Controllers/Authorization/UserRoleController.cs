using IdentityService.Application.Commands;
using IdentityService.Application.Models;
using IdentityService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authorization;

[ApiController]
[Route("api/roles")]
[Authorize]
public class UserRoleController(IMediator mediator) : ControllerBase
{
    [Authorize(Policy = "CanViewRole")]
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetAllRolesQuery(), cancellationToken);
        return Ok(result);
    }

    [Authorize(Policy = "CanViewRole")]
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize(Policy = "CanEditRole")]
    [HttpPost]
    public async Task<IActionResult> Create(RoleDto dto, CancellationToken cancellationToken = default)
    {
        var id = await mediator.Send(new CreateRoleCommand(dto.Name, dto.PermissionIds), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [Authorize(Policy = "CanEditRole")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RoleDto dto, CancellationToken cancellationToken = default)
    {
        var success = await mediator.Send(new UpdateRoleCommand(id, dto.Name, dto.PermissionIds), cancellationToken);
        return success ? NoContent() : NotFound();
    }

    [Authorize(Policy = "CanDeleteRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var success = await mediator.Send(new DeleteRoleCommand(id), cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
