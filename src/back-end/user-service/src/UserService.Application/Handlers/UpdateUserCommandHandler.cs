using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class UpdateUserCommandHandler(IUserRepository repo)
    : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUserRepository _repo = repo;

    public async Task<Unit> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var user =
            await _repo.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"User with ID {request.Id} not found.");
        user.Name = request.Name;
        user.Email = request.Email;

        await _repo.UpdateAsync(user, cancellationToken);
        return Unit.Value;
    }
}
