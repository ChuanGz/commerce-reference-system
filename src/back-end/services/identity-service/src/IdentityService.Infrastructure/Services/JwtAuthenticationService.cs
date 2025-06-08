using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Services
{
    public class JwtAuthenticationService(
        IUserRepository userRepository,
        IConfiguration config,
        ILogger<JwtAuthenticationService> logger
    ) : IAuthenticationService
    {
        public async Task<string?> AuthenticateAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default
        )
        {
            logger.LogInformation("Attempting authentication for user: {Username}", username);

            var user = await userRepository.GetByUsernameAsync(username, cancellationToken);
            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                logger.LogWarning("Authentication failed for user: {Username}", username);
                return null;
            }

            logger.LogInformation("Authentication successful for user: {Username}", username);

            var permissions = user
                .UserGroups.SelectMany(ug => ug.Group.GroupRoles)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            logger.LogInformation("JWT token generated for user: {Username}", username);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(config["Jwt:Secret"]!);

                tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = config["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero,
                    },
                    out SecurityToken validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var permissions = jwtToken
                    .Claims.Where(x => x.Type == "permission")
                    .Select(x => x.Value);

                return await Task.FromResult(
                    new TokenValidationResult
                    {
                        IsValid = true,
                        UserId = userId,
                        Permissions = permissions,
                    }
                );
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Token validation failed");
                return new TokenValidationResult { IsValid = false };
            }
        }

        public async Task RevokeTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        )
        {
            logger.LogInformation("Token revocation requested");
            await Task.CompletedTask;
        }
    }
}
