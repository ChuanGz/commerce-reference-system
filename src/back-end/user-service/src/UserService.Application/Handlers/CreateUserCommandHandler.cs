using UserService.Application.Commands;
using Microsoft.Extensions.Logging;

namespace UserService.Application.Handlers;

public class CreateUserCommandHandler(IUserRepository repo, ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repo = repo;
    private readonly ILogger<CreateUserCommandHandler> _logger = logger;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Creating new user with email: {Email}", request.Email);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = request.Email.Trim()
        };

        await _repo.AddAsync(user, cancellationToken);
        
        _logger.LogInformation("Successfully created user with ID: {UserId}", user.Id);
        return user.Id;
    }
}
