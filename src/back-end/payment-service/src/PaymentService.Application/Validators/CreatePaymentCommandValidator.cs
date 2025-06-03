using FluentValidation;
using PaymentService.Application.Commands;
using PaymentService.Domain.Constants;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Validators;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    private readonly IPaymentRepository _repo;

    public CreatePaymentCommandValidator(IPaymentRepository repo)
    {
        _repo = repo;

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required")
            .MustAsync(async (orderId, cancellationToken) =>
            {
                return !await _repo.AnyAsync(p => p.OrderId == orderId, cancellationToken);
            })
            .WithMessage("Payment for this order already exists");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0")
            .LessThanOrEqualTo(100000)
            .WithMessage("Amount cannot exceed 100,000");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("PaymentMethod is required")
            .Length(2, 50)
            .WithMessage("PaymentMethod must be between 2 and 50 characters")
            .Must(method => new[] { "Credit Card", "Debit Card", "PayPal", "Bank Transfer", "Cash" }.Contains(method))
            .WithMessage("PaymentMethod must be one of: Credit Card, Debit Card, PayPal, Bank Transfer, Cash");
    }
}
