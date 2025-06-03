using FluentValidation;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Validators;

public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity must be greater than or equal to 0");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required")
            .Length(2, 100)
            .WithMessage("Location must be between 2 and 100 characters");
    }
}
