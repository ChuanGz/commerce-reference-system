using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UpdateUserCommandValidator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        RuleFor(x => x.Id).NotEmpty().WithMessage("User Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must be at most 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.")
            .MaximumLength(255)
            .WithMessage("Email must be at most 255 characters.");

        RuleFor(x => x)
            .CustomAsync(
                async (command, context, cancellationToken) =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var normalizedName = command.Name?.Trim().ToUpper() ?? string.Empty;
                    var normalizedEmail = command.Email?.Trim().ToUpper() ?? string.Empty;

                    var nameExists = await repo.AnyAsync(
                        u =>
                            u.Id != command.Id && (u.Name ?? "").Trim().ToUpper() == normalizedName,
                        cancellationToken
                    );

                    if (nameExists)
                        context.AddFailure(
                            nameof(command.Name),
                            $"Name `{command.Name}` already exists."
                        );

                    var emailExists = await repo.AnyAsync(
                        u =>
                            u.Id != command.Id
                            && (u.Email ?? "").Trim().ToUpper() == normalizedEmail,
                        cancellationToken
                    );

                    if (emailExists)
                        context.AddFailure(
                            nameof(command.Email),
                            $"Email `{command.Email}` already exists."
                        );
                }
            );
    }
}
