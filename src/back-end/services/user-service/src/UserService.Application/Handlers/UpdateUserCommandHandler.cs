using UserService.Application.Commands;

namespace UserService.Application.Handlers
{
    public class UpdateUserCommandHandler(IUserRepository repo)
        : IRequestHandler<UpdateUserCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(command);

            var user =
                await repo.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new InvalidOperationException($"User with ID {command.Id} not found.");
            user.Name = command.Name;
            user.Email = command.Email;

            await repo.UpdateAsync(user, cancellationToken);
            return Unit.Value;
        }
    }
}
