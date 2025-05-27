using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authorization;
[ApiController]
[Route("api/roles/{roleId:guid}/permissions")]
[Authorize]
public class RolePermissionController(IdentityDbContext db) : ControllerBase
{
    [Authorize(Policy = "CanViewRole")]
    [HttpGet]
    public async Task<IActionResult> Get(Guid roleId,CancellationToken cancellationToken = default)
    {
        var permissions = await db.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Select(
                rp =>
                    new
                    {
                        rp.PermissionId,
                        rp.Permission.Key,
                        rp.Permission.Description
                    }
            )
            .ToListAsync(cancellationToken);

        return Ok(permissions);
    }
    [Authorize(Policy = "CanAssignRolePermission")]
    [HttpPost("{permissionId:guid}")]
    public async Task<IActionResult> AddPermission(
            Guid roleId,
            Guid permissionId,
            CancellationToken cancellationToken
        )
    {
        var exists = await db.RolePermissions.AnyAsync(
            rp => rp.RoleId == roleId && rp.PermissionId == permissionId,
            cancellationToken
        );

        if (exists)
            return Conflict("Permission already assigned to role.");

        db.RolePermissions.Add(
            new Domain.Entities.RolePermission { RoleId = roleId, PermissionId = permissionId }
        );

        await db.SaveChangesAsync(cancellationToken);
        return Ok("Permission assigned.");
    }
    [Authorize(Policy = "CanAssignRolePermission")]
    [HttpDelete("{permissionId:guid}")]
    public async Task<IActionResult> RemovePermission(
            Guid roleId,
            Guid permissionId,
            CancellationToken cancellationToken
        )
    {
        var existing = await db.RolePermissions.FirstOrDefaultAsync(
            rp => rp.RoleId == roleId && rp.PermissionId == permissionId,
            cancellationToken
        );

        if (existing is null)
            return NotFound("Permission not assigned to this role.");

        db.RolePermissions.Remove(existing);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
