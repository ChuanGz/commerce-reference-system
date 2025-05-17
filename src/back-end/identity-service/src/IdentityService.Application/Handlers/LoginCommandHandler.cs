using IdentityService.Application.Commands;
using IdentityService.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Application.Handlers;

/// <summary>
/// Handles LoginCommand by validating credentials and returning a JWT token with embedded claims.
/// </summary>
public class LoginCommandHandler(IUserRepository userRepository, IConfiguration _config) : IRequestHandler<LoginCommand, string?>
{
    public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        // 🔒 Replace mock password check with real hash verification (BCrypt)
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        // 📄 Flatten permissions from user → groups → roles → permissions
        var permissions = user.UserGroups
            .SelectMany(ug => ug.Group.GroupRoles)
            .SelectMany(gr => gr.Role.RolePermissions)
            .Select(rp => rp.Permission.Key)
            .Distinct()
            .ToList();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
        };

        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

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
