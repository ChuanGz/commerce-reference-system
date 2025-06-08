using IdentityService.Application.Commands;
using IdentityService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authorization
{
    [ApiController]
    [Route("api/roles/{roleId:guid}/permissions")]
    [Authorize]
    public class RolePermissionsController(IMediator mediator) : ControllerBase
    {
        [Authorize(Policy = "CanViewRole")]
        [HttpGet]
        public async Task<IActionResult> Get(
            Guid roleId,
            CancellationToken cancellationToken = default
        )
        {
            var result = await mediator.Send(
                new GetPermissionsByRoleQuery(roleId),
                cancellationToken
            );
            return Ok(result);
        }

        [Authorize(Policy = "CanAssignRolePermission")]
        [HttpPost("{permissionId:guid}")]
        public async Task<IActionResult> Add(
            Guid roleId,
            Guid permissionId,
            CancellationToken cancellationToken = default
        )
        {
            var success = await mediator.Send(
                new AddRolePermissionCommand(roleId, permissionId),
                cancellationToken
            );
            return success ? Ok("Permission assigned.") : Conflict("Permission already assigned.");
        }

        [Authorize(Policy = "CanAssignRolePermission")]
        [HttpDelete("{permissionId:guid}")]
        public async Task<IActionResult> Remove(
            Guid roleId,
            Guid permissionId,
            CancellationToken cancellationToken = default
        )
        {
            var success = await mediator.Send(
                new RemoveRolePermissionCommand(roleId, permissionId),
                cancellationToken
            );
            return success ? NoContent() : NotFound("Permission not assigned to this role.");
        }
    }
}
