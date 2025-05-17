using IdentityService.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize(Policy = "CanViewPermission")]
public class PermissionController(IdentityDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PermissionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var permissions = await db.Permissions
            .Select(
                p =>
                    new PermissionDto
                    {
                        Id = p.Id,
                        Key = p.Key,
                        Description = p.Description
                    }
            )
            .ToListAsync(cancellationToken);

        return Ok(permissions);
    }
}
