using UserService.Application.Queries;

namespace UserService.Application.Handlers {
    public class GetUserByIdQueryHandler(IUserRepository repo)
        : IRequestHandler<GetUserByIdQuery, User?> {
        public Task<User?> Handle(
            GetUserByIdQuery query,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(query);
            return repo.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}
