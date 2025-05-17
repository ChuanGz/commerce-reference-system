using IdentityService.API.Models;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

/// <summary>
/// Manages User-Group assignments (many-to-many).
/// Supports CRUD and approval logic.
/// </summary>
[ApiController]
[Route("api/usergroups")]
[Authorize]
public class UserGroupController : ControllerBase
{
    private readonly IdentityDbContext _db;

    public UserGroupController(IdentityDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get list of all users and their assigned groups.
    /// </summary>
    [Authorize(Policy = "CanViewGroup")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _db.Users
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .Select(
                u =>
                    new UserGroupResponseDto
                    {
                        UserId = u.Id,
                        Username = u.Username,
                        Groups = u.UserGroups.Select(ug => ug.Group.Name).ToList()
                    }
            )
            .ToListAsync(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Assign a user to a group (unapproved by default).
    /// </summary>
    [Authorize(Policy = "CanEditGroup")]
    [HttpPost]
    public async Task<IActionResult> AssignUserToGroup(
        [FromBody] UserGroupDto dto,
        CancellationToken ct
    )
    {
        if (
            await _db.UserGroups.AnyAsync(
                ug => ug.UserId == dto.UserId && ug.GroupId == dto.GroupId,
                ct
            )
        )
            return Conflict("User is already in the group.");

        var userGroup = new UserGroup
        {
            UserId = dto.UserId,
            GroupId = dto.GroupId,
            IsApproved = false
        };

        _db.UserGroups.Add(userGroup);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetAll), null);
    }

    /// <summary>
    /// Approve an existing user-group assignment.
    /// </summary>
    [Authorize(Policy = "CanApproveGroup")]
    [HttpPut("approve")]
    public async Task<IActionResult> Approve([FromBody] UserGroupDto dto, CancellationToken ct)
    {
        var userGroup = await _db.UserGroups.FirstOrDefaultAsync(
            ug => ug.UserId == dto.UserId && ug.GroupId == dto.GroupId,
            ct
        );

        if (userGroup is null)
            return NotFound("Assignment not found.");

        userGroup.IsApproved = true;
        await _db.SaveChangesAsync(ct);

        return Ok("Approved.");
    }

    /// <summary>
    /// Remove a user from a group.
    /// </summary>
    [Authorize(Policy = "CanDeleteGroup")]
    [HttpDelete]
    public async Task<IActionResult> Remove([FromBody] UserGroupDto dto, CancellationToken ct)
    {
        var userGroup = await _db.UserGroups.FirstOrDefaultAsync(
            ug => ug.UserId == dto.UserId && ug.GroupId == dto.GroupId,
            ct
        );

        if (userGroup is null)
            return NotFound("Assignment not found.");

        _db.UserGroups.Remove(userGroup);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }
}
