using UserService.Application.Queries;

namespace UserService.Application.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IUserRepository _repo;

    public GetUserByIdQueryHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) =>
        _repo.GetByIdAsync(request.Id);
}
