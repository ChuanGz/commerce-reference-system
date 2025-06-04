using FluentValidation;
using IdentityService.Application.Commands;

namespace UserService.Application.Validators;

public class UserRegistrationValidator : AbstractValidator<UserRegistrationCommand>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(4).MaximumLength(16);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^[A-Za-z]")
            .Matches(@"[a-z]")
            .Matches(@"\d")
            .Matches(@"[^\w\d\s]")
            .WithMessage(
                "Password must start with a letter, and contain at least one uppercase letter, one lowercase letter, one digit, and one special character."
            );
    }
}
