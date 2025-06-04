using CustomerService.Application.Commands;
using FluentValidation;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required.")
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required.")
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone is required.")
            .MinimumLength(10)
            .MaximumLength(20);

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MinimumLength(10)
            .MaximumLength(200);
    }
}
