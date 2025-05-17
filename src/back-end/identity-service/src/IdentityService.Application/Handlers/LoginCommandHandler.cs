using IdentityService.Application.Commands;
using IdentityService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Application.Handlers;

/// <summary>
/// Handles LoginCommand by validating credentials and returning a JWT token with embedded claims.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, string?>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IConfiguration _config;

    public LoginCommandHandler(IdentityDbContext dbContext, IConfiguration config)
    {
        _dbContext = dbContext;
        _config = config;
    }

    public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // TODO: Replace this with secure password verification (e.g. BCrypt)
        var user = await _dbContext.Users
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.GroupRoles)
            .ThenInclude(gr => gr.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        // Mock password check (replace this!)
        if (user is null || request.Password != "password")
            return null;

        // Flatten permissions from Group → Role → Permission
        var permissions = user.UserGroups
            .SelectMany(ug => ug.Group.GroupRoles)
            .SelectMany(gr => gr.Role.RolePermissions)
            .Select(rp => rp.Permission.Key)
            .Distinct()
            .ToList();

        // Create standard + custom JWT claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
        };

        // Add each permission as a separate claim
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        // Sign the token using JWT secret
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
