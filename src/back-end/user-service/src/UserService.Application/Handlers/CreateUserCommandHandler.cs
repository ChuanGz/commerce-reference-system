using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public class CreateUserCommandHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repo = repo;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate Name
        var nameExists = await _repo.AnyAsync(u => u.Name.Normalize().Trim().Equals(request.Name.Trim(), StringComparison.InvariantCultureIgnoreCase), cancellationToken);
        if (nameExists)
            throw new ArgumentException
("A user with this name already exists.");

        // Check for duplicate Email
        var emailExists = await _repo.AnyAsync(u => u.Email.Normalize().Trim().Equals(request.Email.Trim(), StringComparison.InvariantCultureIgnoreCase), cancellationToken);
        if (emailExists)
            throw new ArgumentException
("A user with this email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email
        };

        await _repo.AddAsync(user, cancellationToken);
        return user.Id;
    }
}
