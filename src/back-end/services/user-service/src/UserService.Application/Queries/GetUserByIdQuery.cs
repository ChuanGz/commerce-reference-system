namespace UserService.Application.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<User?>;
}
