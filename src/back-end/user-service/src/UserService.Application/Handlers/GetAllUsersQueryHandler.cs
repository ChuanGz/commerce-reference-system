using UserService.Application.Queries;

namespace UserService.Application.Handlers;

public class GetAllUsersQueryHandler(IUserRepository repo)
    : IRequestHandler<GetAllUsersQuery, List<User>>
{
    private readonly IUserRepository _repo = repo;

    public Task<List<User>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken = default
    ) => _repo.GetAllAsync(cancellationToken);
}
