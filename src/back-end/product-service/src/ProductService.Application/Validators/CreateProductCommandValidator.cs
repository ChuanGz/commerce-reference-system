using FluentValidation;
using ProductService.Application.Commands;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IProductRepository _repo;

    public CreateProductCommandValidator(IProductRepository repo)
    {
        _repo = repo;

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

        RuleFor(x => x.SKU)
            .NotEmpty()
            .WithMessage("SKU is required")
            .Length(3, 20)
            .WithMessage("SKU must be between 3 and 20 characters")
            .MustAsync(
                async (sku, cancellationToken) =>
                {
                    return !await _repo.AnyAsync(p => p.SKU == sku.ToUpper(), cancellationToken);
                }
            )
            .WithMessage("SKU already exists");
    }
}
