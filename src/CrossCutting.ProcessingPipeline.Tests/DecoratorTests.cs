namespace CrossCutting.ProcessingPipeline.Tests;

public class DecoratorTests
{
    [Fact]
    public async Task Can_Decorate_PipelineComponents()
    {
        // Arrange
        var decorator = new LoggerDecorator(new ExceptionDecorator(new PassThroughDecorator()));
        var sut = new Pipeline<DecoratorTestsContext>(decorator, [new MyComponent()]);
        var context = new DecoratorTestsContext();

        // Act
        var result = await sut.ExecuteAsync(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        context.ToString().ShouldBe(@"--- Start ---
MyComponent called
--- End ---
");

    }

    private sealed class MyComponent : IPipelineComponent<DecoratorTestsContext>
    {
        public Task<Result> ExecuteAsync(DecoratorTestsContext command, CancellationToken token)
            => Task.Run(() =>
            {
                command.AppendLine("MyComponent called");

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

        public async Task<Result> ExecuteAsync<TCommand>(IPipelineComponent<TCommand> component, TCommand command, CancellationToken token)
        {
            try
            {
                if (command is DecoratorTestsContext dtc)
                {
                    dtc.AppendLine("--- Start ---");
                }
                return await ((Func<Task<Result>>)(() => _decorator.ExecuteAsync(component, command, token)))().ConfigureAwait(false);
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

        public async Task<Result> ExecuteAsync<TCommand>(IPipelineComponent<TCommand> component, TCommand command, CancellationToken token)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return await ((Func<Task<Result>>)(() => _decorator.ExecuteAsync(component, command, token)))().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Result.Error(ex, "Error occured, see Exception for more details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
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
