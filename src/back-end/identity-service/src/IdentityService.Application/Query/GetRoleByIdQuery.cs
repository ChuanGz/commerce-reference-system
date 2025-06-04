using IdentityService.Application.Models;

namespace IdentityService.Application.Queries;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto?>;
