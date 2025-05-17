using IdentityService.API.Models;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IdentityDbContext _db;

    public RoleController(IdentityDbContext db)
    {
        _db = db;
    }

    // GET: /api/roles
    [Authorize(Policy = "CanViewRole")]
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll(CancellationToken ct)
    {
        var roles = await _db.Roles
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
            .ToListAsync(ct);

        return Ok(roles);
    }

    // GET: /api/roles/{id}
    [Authorize(Policy = "CanViewRole")]
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(Guid id, CancellationToken ct)
    {
        var role = await _db.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (role is null)
            return NotFound();

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList()
        };
    }

    // POST: /api/roles
    [Authorize(Policy = "CanEditRole")]
    [HttpPost]
    public async Task<IActionResult> Create(RoleDto dto, CancellationToken ct)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            RolePermissions = dto.PermissionIds
                .Select(pid => new RolePermission { PermissionId = pid })
                .ToList()
        };

        _db.Roles.Add(role);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = role.Id }, null);
    }

    // PUT: /api/roles/{id}
    [Authorize(Policy = "CanEditRole")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RoleDto dto, CancellationToken ct)
    {
        var role = await _db.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (role is null)
            return NotFound();

        role.Name = dto.Name;

        // Remove old permissions
        _db.RolePermissions.RemoveRange(role.RolePermissions);

        // Add new ones
        role.RolePermissions = dto.PermissionIds
            .Select(pid => new RolePermission { RoleId = role.Id, PermissionId = pid })
            .ToList();

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // DELETE: /api/roles/{id}
    [Authorize(Policy = "CanDeleteRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);

        if (role is null)
            return NotFound();

        _db.Roles.Remove(role);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}
