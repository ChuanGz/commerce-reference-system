using FluentValidation;
using OrderService.Application.Commands;

namespace OrderService.Application.Validators;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(status =>
                new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" }.Contains(
                    status
                )
            )
            .WithMessage(
                "Status must be one of: Pending, Processing, Shipped, Delivered, Cancelled"
            );
    }
}
