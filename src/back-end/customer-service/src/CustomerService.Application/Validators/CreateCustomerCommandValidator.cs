using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using CustomerService.Application.Commands;
using CustomerService.Domain.Repositories;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CreateCustomerCommandValidator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MinimumLength(10)
            .MaximumLength(20);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MinimumLength(10)
            .MaximumLength(200);

        RuleFor(x => x).CustomAsync(async (command, context, cancellationToken) =>
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

            var userIdExists = await repo.AnyAsync(
                c => c.UserId == command.UserId,
                cancellationToken);

            if (userIdExists)
                context.AddFailure(nameof(command.UserId), $"Customer with UserId `{command.UserId}` already exists.");
        });
    }
}
