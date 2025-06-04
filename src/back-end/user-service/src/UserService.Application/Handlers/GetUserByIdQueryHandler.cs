using UserService.Application.Queries;

namespace UserService.Application.Handlers;

public class GetUserByIdQueryHandler(IUserRepository repo)
    : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IUserRepository _repo = repo;

    public Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default) {
        ArgumentNullException.ThrowIfNull(query);
        return _repo.GetByIdAsync(query.Id, cancellationToken);
    }
}
