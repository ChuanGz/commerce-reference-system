using MediatR;
using UserService.Application.Commands;
using UserService.Domain.Constants;
using UserService.Domain.Constants;
using UserService.Domain.Repositories;

namespace UserService.Application.Handlers;

public class DeleteUserCommandHandler(IUserRepository repo) : IRequestHandler<DeleteUserCommand>
{
    public async Task<Unit> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(command);
        var user =
            await repo.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new ArgumentException(ErrorMessages.UserNotFound);
        await repo.DeleteAsync(user.Id, cancellationToken);
        return Unit.Value;
    }
}
