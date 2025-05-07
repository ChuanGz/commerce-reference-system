using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo;

    public DeleteUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
