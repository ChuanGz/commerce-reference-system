using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class CreateUserCommandHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repo = repo;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = request.Email.Trim()
        };


        await _repo.AddAsync(user, cancellationToken);
        return user.Id;
    }
}
