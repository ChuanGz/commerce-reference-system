using UserService.Application.Queries;

namespace UserService.Application.Handlers
{
    public class GetAllUsersQueryHandler(IUserRepository repo)
        : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        public Task<List<User>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken = default
        ) => repo.GetAllAsync(cancellationToken);
    }
}
