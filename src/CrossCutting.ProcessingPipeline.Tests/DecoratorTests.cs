namespace CrossCutting.ProcessingPipeline.Tests;

public class DecoratorTests
{
    [Fact]
    public async Task Can_Decorate_PipelineComponents_Responseless()
    {
        // Arrange
        var decorator = new LoggerDecorator(new ExceptionDecorator(new PassThroughDecorator()));
        var sut = new PipelineHandler<DecoratorTestsContext>(decorator, [new MyComponent()]);
        var context = new DecoratorTestsContext();
        var commandService = Substitute.For<ICommandService>();

        // Act
        var result = await sut.ExecuteAsync(context, commandService, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        context.ToString().ShouldBe(@"--- Start ---
MyComponent called
--- End ---
");

    }

    [Fact]
    public async Task Can_Decorate_PipelineComponents_With_Response()
    {
        // Arrange
        var decorator = new LoggerDecorator(new ExceptionDecorator(new PassThroughDecorator()));
        var responseGenerator = new PipelineResponseGenerator([]);
        var sut = new PipelineHandler<DecoratorTestsContext, StringBuilder>(decorator, responseGenerator, [new MyStringBuilderComponent()]);
        var context = new DecoratorTestsContext();
        var commandService = Substitute.For<ICommandService>();

        // Act
        var result = await sut.ExecuteAsync(context, commandService, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.ToString().ShouldBe("My Result");
        context.ToString().ShouldBe(@"--- Start ---
MyComponent called
--- End ---
");

    }

    private sealed class MyComponent : IPipelineComponent<DecoratorTestsContext>
    {
        public Task<Result> ExecuteAsync(DecoratorTestsContext command, ICommandService commandService, CancellationToken token)
            => Task.Run(() =>
            {
                command.AppendLine("MyComponent called");

                return Result.Success();
            }, token);
    }

    private sealed class MyStringBuilderComponent : IPipelineComponent<DecoratorTestsContext, StringBuilder>
    {
        public Task<Result> ExecuteAsync(DecoratorTestsContext command, StringBuilder response, ICommandService commandService, CancellationToken token)
            => Task.Run(() =>
            {
                command.AppendLine("MyComponent called");
                response.Append("My Result");

                return Result.Success();
            }, token);
    }

    private sealed class LoggerDecorator : IPipelineComponentDecorator
    {
        private readonly IPipelineComponentDecorator _decorator;

        public LoggerDecorator(IPipelineComponentDecorator decorator)
        {
            ArgumentGuard.IsNotNull(decorator, nameof(decorator));

            _decorator = decorator;
        }

        public async Task<Result> ExecuteAsync<TCommand>(IPipelineComponent<TCommand> component, TCommand command, ICommandService commandService, CancellationToken token)
        {
            try
            {
                if (command is DecoratorTestsContext dtc)
                {
                    dtc.AppendLine("--- Start ---");
                }
                return await ((Func<Task<Result>>)(() => _decorator.ExecuteAsync(component, command, commandService, token)))().ConfigureAwait(false);
            }
            finally
            {
                if (command is DecoratorTestsContext dtc)
                {
                    dtc.AppendLine("--- End ---");
                }
            }
        }

        public async Task<Result> ExecuteAsync<TCommand, TResponse>(IPipelineComponent<TCommand, TResponse> component, TCommand command, TResponse response, ICommandService commandService, CancellationToken token)
        {
            try
            {
                if (command is DecoratorTestsContext dtc)
                {
                    dtc.AppendLine("--- Start ---");
                }
                return await((Func<Task<Result>>)(() => _decorator.ExecuteAsync(component, command, response, commandService, token)))().ConfigureAwait(false);
            }
            finally
            {
                if (command is DecoratorTestsContext dtc)
                {
                    dtc.AppendLine("--- End ---");
                }
            }
        }
    }

    private sealed class ExceptionDecorator : IPipelineComponentDecorator
    {
        private readonly IPipelineComponentDecorator _decorator;

        public ExceptionDecorator(IPipelineComponentDecorator decorator)
        {
            ArgumentGuard.IsNotNull(decorator, nameof(decorator));

            _decorator = decorator;
        }

        public async Task<Result> ExecuteAsync<TCommand>(IPipelineComponent<TCommand> component, TCommand command, ICommandService commandService, CancellationToken token)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return await ((Func<Task<Result>>)(() => _decorator.ExecuteAsync(component, command, commandService, token)))().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Result.Error(ex, "Error occured, see Exception for more details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        public async Task<Result> ExecuteAsync<TCommand, TResponse>(IPipelineComponent<TCommand, TResponse> component, TCommand command, TResponse response, ICommandService commandService, CancellationToken token)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return await ((Func<Task<Result>>)(() => _decorator.ExecuteAsync(component, command, response, commandService, token)))().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Result.Error(ex, "Error occured, see Exception for more details");
            }
#pragma warning restore CA1031 // Do not catch general exception types        }
        }
    }

    private sealed class DecoratorTestsContext : IValidatableObject
    {
        private readonly StringBuilder _builder = new();

        public void AppendLine(string value)
            => _builder.AppendLine(value);

        public override string ToString()
            => _builder.ToString();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            => Enumerable.Empty<ValidationResult>();
    }
}
