using FluentValidation;
using ProductService.Application.Commands;

namespace ProductService.Application.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .Length(2, 100)
            .WithMessage("Name must be between 2 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .Length(10, 500)
            .WithMessage("Description must be between 10 and 500 characters");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .Length(2, 50)
            .WithMessage("Category must be between 2 and 50 characters");
    }
}
