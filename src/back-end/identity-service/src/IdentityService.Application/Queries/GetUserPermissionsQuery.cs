using IdentityService.Application.Models;

namespace IdentityService.Application.Queries;

public record GetUserPermissionsQuery(Guid UserId) : IRequest<IEnumerable<string>>;
