using IdentityService.Application.Commands;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Handlers;

public class UserRegistrationCommandHandler(
    IUserRepository userRepository,
    IGroupRepository groupRepository,
    IUserGroupRepository userGroupRepository) : IRequestHandler<UserRegistrationCommand, UserRegistrationResult?>
{
    public async Task<UserRegistrationResult?> Handle(UserRegistrationCommand request, CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
            return null;

        var customerGroup = await groupRepository.GetByNameAsync("Customer", cancellationToken);
        if (customerGroup is null)
            throw new InvalidOperationException("Default group not found.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = request.Password
        };

        await userRepository.AddAsync(user, cancellationToken);

        var userGroup = new UserGroup
        {
            UserId = user.Id,
            GroupId = customerGroup.Id
        };

        await userGroupRepository.AddAsync(userGroup, cancellationToken);

        return new UserRegistrationResult(user.Id, user.Username);
    }
}
