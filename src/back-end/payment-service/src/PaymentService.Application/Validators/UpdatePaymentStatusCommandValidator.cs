using FluentValidation;
using PaymentService.Application.Commands;
using PaymentService.Domain.Constants;

namespace PaymentService.Application.Validators;

public class UpdatePaymentStatusCommandValidator : AbstractValidator<UpdatePaymentStatusCommand>
{
    public UpdatePaymentStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(status => new[] { PaymentStatus.Pending, PaymentStatus.Completed, PaymentStatus.Failed, PaymentStatus.Cancelled, PaymentStatus.Refunded }.Contains(status))
            .WithMessage("Status must be one of: Pending, Completed, Failed, Cancelled, Refunded");

        RuleFor(x => x.TransactionId)
            .Length(5, 100)
            .WithMessage("TransactionId must be between 5 and 100 characters")
            .When(x => !string.IsNullOrEmpty(x.TransactionId));
    }
}
