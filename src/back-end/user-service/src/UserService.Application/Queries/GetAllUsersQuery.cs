namespace UserService.Application.Queries;

public record GetAllUsersQuery() : IRequest<List<User>>;
