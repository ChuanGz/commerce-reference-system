using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
    private readonly IServiceScopeFactory _scopeFactory;

    public CreateUserCommandValidator(IServiceScopeFactory scopeFactory) {
        _scopeFactory = scopeFactory;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(5)
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(255)
            .Matches(
                @"^(?:[a-zA-Z0-9_'^&/+-])+(?:\.(?:[a-zA-Z0-9_'^&/+-])+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$"
            );

        RuleFor(x => x)
            .CustomAsync(
                async (command, context, cancellationToken) => {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var normalizedName = command.Name?.Trim().ToUpper() ?? string.Empty;
                    var normalizedEmail = command.Email?.Trim().ToUpper() ?? string.Empty;

                    var nameExists = await repo.AnyAsync(
                        u => (u.Name ?? "").Trim().ToUpper() == normalizedName,
                        cancellationToken
                    );

                    if (nameExists)
                        context.AddFailure(
                            nameof(command.Name),
                            $"Name `{command.Name}` already exists."
                        );

                    var emailExists = await repo.AnyAsync(
                        u => (u.Email ?? "").Trim().ToUpper() == normalizedEmail,
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
