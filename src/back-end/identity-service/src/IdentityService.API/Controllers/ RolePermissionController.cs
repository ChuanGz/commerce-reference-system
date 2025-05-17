using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

/// <summary>
/// Manages assignment of permissions to roles.
/// </summary>
[ApiController]
[Route("api/roles/{roleId:guid}/permissions")]
[Authorize]
public class RolePermissionController : ControllerBase
{
    private readonly IdentityDbContext _db;

    public RolePermissionController(IdentityDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// List all permission keys assigned to a specific role.
    /// </summary>
    [Authorize(Policy = "CanViewRole")]
    [HttpGet]
    public async Task<IActionResult> Get(Guid roleId, CancellationToken ct)
    {
        var permissions = await _db.RolePermissions
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
            .ToListAsync(ct);

        return Ok(permissions);
    }

    /// <summary>
    /// Assign a permission to a role.
    /// </summary>
    [Authorize(Policy = "CanAssignRolePermission")]
    [HttpPost("{permissionId:guid}")]
    public async Task<IActionResult> AddPermission(
        Guid roleId,
        Guid permissionId,
        CancellationToken ct
    )
    {
        var exists = await _db.RolePermissions.AnyAsync(
            rp => rp.RoleId == roleId && rp.PermissionId == permissionId,
            ct
        );

        if (exists)
            return Conflict("Permission already assigned to role.");

        _db.RolePermissions.Add(
            new Domain.Entities.RolePermission { RoleId = roleId, PermissionId = permissionId }
        );

        await _db.SaveChangesAsync(ct);
        return Ok("Permission assigned.");
    }

    /// <summary>
    /// Remove a permission from a role.
    /// </summary>
    [Authorize(Policy = "CanAssignRolePermission")]
    [HttpDelete("{permissionId:guid}")]
    public async Task<IActionResult> RemovePermission(
        Guid roleId,
        Guid permissionId,
        CancellationToken ct
    )
    {
        var existing = await _db.RolePermissions.FirstOrDefaultAsync(
            rp => rp.RoleId == roleId && rp.PermissionId == permissionId,
            ct
        );

        if (existing is null)
            return NotFound("Permission not assigned to this role.");

        _db.RolePermissions.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}
