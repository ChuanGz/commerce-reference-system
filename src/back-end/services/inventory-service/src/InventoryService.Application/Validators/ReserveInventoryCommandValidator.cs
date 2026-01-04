using FluentValidation;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Validators {
    public class ReserveInventoryCommandValidator : AbstractValidator<ReserveInventoryCommand> {
        public ReserveInventoryCommandValidator() {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");

            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}
