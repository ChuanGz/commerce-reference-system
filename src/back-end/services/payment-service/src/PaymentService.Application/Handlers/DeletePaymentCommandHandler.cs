using PaymentService.Application.Commands;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Handlers
{
    public class DeletePaymentCommandHandler(IPaymentRepository repo)
        : IRequestHandler<DeletePaymentCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeletePaymentCommand request,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(request);

            await repo.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
