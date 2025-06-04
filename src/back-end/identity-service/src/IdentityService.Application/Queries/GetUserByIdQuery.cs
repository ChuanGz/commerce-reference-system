using IdentityService.Domain.Entities;

namespace IdentityService.Application.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<User?>;
