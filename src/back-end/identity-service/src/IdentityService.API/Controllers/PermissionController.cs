using IdentityService.API.Models;
using IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize(Policy = "CanViewPermission")]
public class PermissionController : ControllerBase
{
    private readonly IdentityDbContext _db;

    public PermissionController(IdentityDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<PermissionDto>>> GetAll(CancellationToken ct)
    {
        var permissions = await _db.Permissions
            .Select(
                p =>
                    new PermissionDto
                    {
                        Id = p.Id,
                        Key = p.Key,
                        Description = p.Description
                    }
            )
            .ToListAsync(ct);

        return Ok(permissions);
    }
}
