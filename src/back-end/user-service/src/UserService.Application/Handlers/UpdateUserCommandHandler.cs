using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUserRepository _repo;

    public UpdateUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repo.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new Exception($"User with ID {request.Id} not found.");
        }

        user.Name = request.Name;
        user.Email = request.Email;

        await _repo.UpdateAsync(user);
        return Unit.Value;
    }
}
