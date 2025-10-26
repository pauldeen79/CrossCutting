namespace CrossCutting.Commands.Tests;

public class CommandServiceTests
{
    public sealed class MyCommand { }
    public sealed class WrongCommand { }
    public sealed class MyResponse { }

    public class ExecuteAsync_Command : CommandServiceTests
    {
        [Fact]
        public async Task Returns_NotSupported_When_Command_Has_No_Handler()
        {
            // Arrange
            var handler = Substitute.For<ICommandHandler<WrongCommand>>();
            var command = new MyCommand();
            var sut = new CommandService(new PassThroughDecorator(), [handler]);

            // Act
            var result = await sut.ExecuteAsync(command);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("No command handler is known for command type CrossCutting.Commands.Tests.CommandServiceTests+MyCommand");
        }

        [Fact]
        public async Task Returns_NotSupported_When_Command_Has_Multiple_Handlers()
        {
            // Arrange
            var handler = Substitute.For<ICommandHandler<MyCommand>>();
            var command = new MyCommand();
            var sut = new CommandService(new PassThroughDecorator(), [handler, handler]);

            // Act
            var result = await sut.ExecuteAsync(command);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("2 command handlers are known for command type CrossCutting.Commands.Tests.CommandServiceTests+MyCommand, only 1 can be present");
        }

        [Fact]
        public async Task Returns_Result_From_Handler_When_Exactly_One_Is_Found()
        {
            // Arrange
            var handler = Substitute.For<ICommandHandler<MyCommand>>();
            handler.ExecuteAsync(Arg.Any<MyCommand>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var command = new MyCommand();
            var sut = new CommandService(new PassThroughDecorator(), [handler]);

            // Act
            var result = await sut.ExecuteAsync(command);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }

    public class ExecuteAsync_Command_Response : CommandServiceTests
    {
        [Fact]
        public async Task Returns_NotSupported_When_Command_Has_No_Handler()
        {
            // Arrange
            var handler = Substitute.For<ICommandHandler<WrongCommand, MyResponse>>();
            var command = new MyCommand();
            var sut = new CommandService(new PassThroughDecorator(), [handler]);

            // Act
            var result = await sut.ExecuteAsync<MyCommand, MyResponse>(command);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("No command handler is known for command type CrossCutting.Commands.Tests.CommandServiceTests+MyCommand");
        }

        [Fact]
        public async Task Returns_NotSupported_When_Command_Has_Multiple_Handlers()
        {
            // Arrange
            var handler = Substitute.For<ICommandHandler<MyCommand, MyResponse>>();
            var command = new MyCommand();
            var sut = new CommandService(new PassThroughDecorator(), [handler, handler]);

            // Act
            var result = await sut.ExecuteAsync<MyCommand, MyResponse>(command);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("2 command handlers are known for command type CrossCutting.Commands.Tests.CommandServiceTests+MyCommand, only 1 can be present");
        }

        [Fact]
        public async Task Returns_Result_From_Handler_When_Exactly_One_Is_Found()
        {
            // Arrange
            var handler = Substitute.For<ICommandHandler<MyCommand, MyResponse>>();
            handler.ExecuteAsync(Arg.Any<MyCommand>(), Arg.Any<CancellationToken>()).Returns(Result.Success(new MyResponse()));
            var command = new MyCommand();
            var sut = new CommandService(new PassThroughDecorator(), [handler]);

            // Act
            var result = await sut.ExecuteAsync<MyCommand, MyResponse>(command);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
