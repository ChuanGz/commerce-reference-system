using IdentityService.API.Models;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RoleController(IdentityDbContext db) : ControllerBase
{
    [Authorize(Policy = "CanViewRole")]
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll(CancellationToken cancellationToken)
    {
        var roles = await db.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Select(
                r =>
                    new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        PermissionIds = r.RolePermissions.Select(rp => rp.PermissionId).ToList()
                    }
            )
            .ToListAsync(cancellationToken);

        return Ok(roles);
    }
    [Authorize(Policy = "CanViewRole")]
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var role = await db.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (role is null)
            return NotFound();

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            PermissionIds = [.. role.RolePermissions.Select(rp => rp.PermissionId)]
        };
    }
    [Authorize(Policy = "CanEditRole")]
    [HttpPost]
    public async Task<IActionResult> Create(RoleDto dto, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            RolePermissions = [.. dto.PermissionIds.Select(pid => new RolePermission { PermissionId = pid })]
        };

        db.Roles.Add(role);
        await db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = role.Id }, null);
    }
    [Authorize(Policy = "CanEditRole")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RoleDto dto, CancellationToken cancellationToken)
    {
        var role = await db.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (role is null)
            return NotFound();

        role.Name = dto.Name;
        db.RolePermissions.RemoveRange(role.RolePermissions);
        role.RolePermissions = [.. dto.PermissionIds.Select(pid => new RolePermission { RoleId = role.Id, PermissionId = pid })];

        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
    [Authorize(Policy = "CanDeleteRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (role is null)
            return NotFound();

        db.Roles.Remove(role);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
