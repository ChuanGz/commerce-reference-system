namespace IdentityService.Application.Commands
{
    public record DeleteRoleCommand(Guid Id) : IRequest<bool>;
}
