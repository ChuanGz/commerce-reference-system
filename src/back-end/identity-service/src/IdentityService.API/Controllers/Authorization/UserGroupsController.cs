using IdentityService.Application.Models;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.Authorization;
[ApiController]
[Route("api/usergroups")]
[Authorize]
public class UserGroupController(IdentityDbContext db) : ControllerBase
{
    [Authorize(Policy = "CanViewGroup")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await db.Users
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
    [Authorize(Policy = "CanEditGroup")]
    [HttpPost]
    public async Task<IActionResult> AssignUserToGroup(
            [FromBody] UserGroupDto dto,
            CancellationToken cancellationToken
        )
    {
        if (
            await db.UserGroups.AnyAsync(
                ug => ug.UserId == dto.UserId && ug.GroupId == dto.GroupId,
                cancellationToken
            )
        )
            return Conflict("User is already in the group.");

        var userGroup = new UserGroup
        {
            UserId = dto.UserId,
            GroupId = dto.GroupId,
            IsApproved = false
        };

        db.UserGroups.Add(userGroup);
        await db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetAll), null);
    }
    [Authorize(Policy = "CanApproveGroup")]
    [HttpPut("approve")]
    public async Task<IActionResult> Approve([FromBody] UserGroupDto dto, CancellationToken cancellationToken)
    {
        var userGroup = await db.UserGroups.FirstOrDefaultAsync(
            ug => ug.UserId == dto.UserId && ug.GroupId == dto.GroupId,
            cancellationToken
        );

        if (userGroup is null)
            return NotFound("Assignment not found.");

        userGroup.IsApproved = true;
        await db.SaveChangesAsync(cancellationToken);

        return Ok("Approved.");
    }
    [Authorize(Policy = "CanDeleteGroup")]
    [HttpDelete]
    public async Task<IActionResult> Remove([FromBody] UserGroupDto dto, CancellationToken cancellationToken)
    {
        var userGroup = await db.UserGroups.FirstOrDefaultAsync(
            ug => ug.UserId == dto.UserId && ug.GroupId == dto.GroupId,
            cancellationToken
        );

        if (userGroup is null)
            return NotFound("Assignment not found.");

        db.UserGroups.Remove(userGroup);
        await db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
