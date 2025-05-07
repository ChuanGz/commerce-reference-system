using Xunit;
using NSubstitute;
using System;
using System.Threading.Tasks;
using UserService.Application.Commands;
using UserService.Application.Handlers;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Application.Tests.Handlers
{
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldAddUserAndReturnId()
        {
            // Arrange
            var repoSub = Substitute.For<IUserRepository>();
            repoSub.AddAsync(Arg.Any<User>()).Returns(Task.CompletedTask);

            var handler = new CreateUserCommandHandler(repoSub);
            var command = new CreateUserCommand("Alice", "alice@mail.com");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            await repoSub
                .Received(1)
                .AddAsync(
                    Arg.Is<User>(
                        u => u.Name == "Alice" && u.Email == "alice@mail.com" && u.Id == result
                    )
                );
        }
    }
}
