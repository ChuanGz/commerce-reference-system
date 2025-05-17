using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class CreateUserCommandHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repo = repo;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email
        };

        await _repo.AddAsync(user);
        return user.Id;
    }
}
