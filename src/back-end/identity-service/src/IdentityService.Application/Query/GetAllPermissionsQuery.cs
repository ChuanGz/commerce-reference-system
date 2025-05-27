using IdentityService.Application.Models;

namespace IdentityService.Application.Queries;

public record GetAllPermissionsQuery() : IRequest<List<PermissionDto>>;
