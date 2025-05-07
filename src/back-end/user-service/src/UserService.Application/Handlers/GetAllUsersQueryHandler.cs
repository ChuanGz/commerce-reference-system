using UserService.Application.Queries;

namespace UserService.Application.Handlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
{
    private readonly IUserRepository _repo;

    public GetAllUsersQueryHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) =>
        _repo.GetAllAsync();
}
