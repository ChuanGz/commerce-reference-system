using MediatR;
using UserService.Application.Commands;
using UserService.Domain.Constants;
using UserService.Domain.Repositories;

namespace UserService.Application.Handlers;

public class DeleteUserCommandHandler(IUserRepository repo) : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo = repo;

    public async Task<Unit> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var user =
            await _repo.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ArgumentException(ErrorMessages.UserNotFound);
        await _repo.DeleteAsync(user.Id, cancellationToken);
        return Unit.Value;
    }
}
