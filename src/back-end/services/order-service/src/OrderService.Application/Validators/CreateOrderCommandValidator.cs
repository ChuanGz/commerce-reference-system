using FluentValidation;
using OrderService.Application.Commands;

namespace OrderService.Application.Validators {
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand> {
        public CreateOrderCommandValidator() {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty()
                .WithMessage("ShippingAddress is required")
                .Length(10, 200)
                .WithMessage("ShippingAddress must be between 10 and 200 characters");

            RuleFor(x => x.OrderItems)
                .NotEmpty()
                .WithMessage("OrderItems are required")
                .Must(items => items.Count > 0)
                .WithMessage("At least one order item is required");

            RuleForEach(x => x.OrderItems)
                .ChildRules(item => {
                    item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");

                    item.RuleFor(x => x.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than 0")
                        .LessThanOrEqualTo(100)
                        .WithMessage("Quantity cannot exceed 100");

                    item.RuleFor(x => x.UnitPrice)
                        .GreaterThan(0)
                        .WithMessage("UnitPrice must be greater than 0");
                });
        }
    }
}
