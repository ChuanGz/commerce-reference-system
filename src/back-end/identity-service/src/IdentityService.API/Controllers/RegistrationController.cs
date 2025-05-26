using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/register")]
[AllowAnonymous]
public class RegistrationController(IdentityDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var exists = await db.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
        if (exists)
            return Conflict("Username already taken.");
        var customerGroup = await db.Groups.FirstOrDefaultAsync(g => g.Name == "Customer", cancellationToken);
        if (customerGroup is null)
            return StatusCode(500, "Default group not found.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = request.Password, // Replace with hashing in production
            UserGroups = [new UserGroup { GroupId = customerGroup.Id }]
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);

        return Created("/api/register", new { user.Id, user.Username });
    }
}

public class RegisterRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
