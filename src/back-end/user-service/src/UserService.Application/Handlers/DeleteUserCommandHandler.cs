using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class DeleteUserCommandHandler(IUserRepository repo) : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo = repo;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
