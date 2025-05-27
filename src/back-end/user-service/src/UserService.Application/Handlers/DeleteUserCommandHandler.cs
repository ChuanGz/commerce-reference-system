using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class DeleteUserCommandHandler(IUserRepository repo) : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo = repo;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = await _repo.GetByIdAsync(request.Id, cancellationToken) ?? throw new ArgumentException("User not found.");
        await _repo.DeleteAsync(user.Id, cancellationToken);
        return Unit.Value;
    }
}
