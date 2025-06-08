using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.Commands
{
    public record UserRegistrationCommand(
        [property: Required] string Username,
        [property: Required, MinLength(6)] string Password
    ) : IRequest<UserRegistrationResult>;

    public record UserRegistrationResult(Guid Id, string Username);
}
