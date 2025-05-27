using IdentityService.Application.Models;

namespace IdentityService.Application.Queries;

public record GetAllRolesQuery() : IRequest<List<RoleDto>>;
