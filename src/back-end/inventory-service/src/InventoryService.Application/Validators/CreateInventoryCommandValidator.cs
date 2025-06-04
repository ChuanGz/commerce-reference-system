using FluentValidation;
using InventoryService.Application.Commands;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Validators;

public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    private readonly IInventoryRepository _repo;

    public CreateInventoryCommandValidator(IInventoryRepository repo)
    {
        _repo = repo;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required")
            .MustAsync(
                async (productId, cancellationToken) =>
                {
                    return !await _repo.AnyAsync(i => i.ProductId == productId, cancellationToken);
                }
            )
            .WithMessage("Inventory for this product already exists");

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
