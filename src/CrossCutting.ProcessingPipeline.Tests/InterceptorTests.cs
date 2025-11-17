namespace CrossCutting.ProcessingPipeline.Tests;

public class InterceptorTests
{
    [Fact]
    public async Task Can_Intercept_PipelineComponents_Responseless()
    {
        // Arrange
        var loggingInterceptor = new LoggerInterceptor();
        var exceptionInterceptor = new ExceptionInterceptor();
        var sut = new PipelineHandler<InterceptorTestsContext>([loggingInterceptor, exceptionInterceptor], [new MyComponent()]);
        var context = new InterceptorTestsContext();
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
    public async Task Can_Intercept_PipelineComponents_With_Response()
    {
        // Arrange
        var loggingInterceptor = new LoggerInterceptor();
        var exceptionInterceptor = new ExceptionInterceptor();
        var responseGenerator = new PipelineResponseGenerator([]);
        var sut = new PipelineHandler<InterceptorTestsContext, StringBuilder>([loggingInterceptor, exceptionInterceptor], responseGenerator, [new MyStringBuilderComponent()]);
        var context = new InterceptorTestsContext();
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

    private sealed class MyComponent : IPipelineComponent<InterceptorTestsContext>
    {
        public Task<Result> ExecuteAsync(InterceptorTestsContext command, ICommandService commandService, CancellationToken token)
            => Task.Run(() =>
            {
                command.AppendLine("MyComponent called");

                return Result.Success();
            }, token);
    }

    private sealed class MyStringBuilderComponent : IPipelineComponent<InterceptorTestsContext, StringBuilder>
    {
        public Task<Result> ExecuteAsync(InterceptorTestsContext command, StringBuilder response, ICommandService commandService, CancellationToken token)
            => Task.Run(() =>
            {
                command.AppendLine("MyComponent called");
                response.Append("My Result");

                return Result.Success();
            }, token);
    }

    private sealed class LoggerInterceptor : IPipelineComponentInterceptor
    {
        public async Task<Result> ExecuteAsync<TCommand>(TCommand command, ICommandService commandService, Func<Task<Result>> next, CancellationToken token)
        {
            try
            {
                if (command is InterceptorTestsContext dtc)
                {
                    dtc.AppendLine("--- Start ---");
                }
                return await next().ConfigureAwait(false);
            }
            finally
            {
                if (command is InterceptorTestsContext dtc)
                {
                    dtc.AppendLine("--- End ---");
                }
            }
        }

        public async Task<Result> ExecuteAsync<TCommand, TResponse>(TCommand command, TResponse response, ICommandService commandService, Func<Task<Result>> next, CancellationToken token)
        {
            try
            {
                if (command is InterceptorTestsContext dtc)
                {
                    dtc.AppendLine("--- Start ---");
                }
                return await next().ConfigureAwait(false);
            }
            finally
            {
                if (command is InterceptorTestsContext dtc)
                {
                    dtc.AppendLine("--- End ---");
                }
            }
        }
    }

    private sealed class ExceptionInterceptor : IPipelineComponentInterceptor
    {
        public async Task<Result> ExecuteAsync<TCommand>(TCommand command, ICommandService commandService, Func<Task<Result>> next, CancellationToken token)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return await next().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Result.Error(ex, "Error occured, see Exception for more details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        public async Task<Result> ExecuteAsync<TCommand, TResponse>(TCommand command, TResponse response, ICommandService commandService, Func<Task<Result>> next, CancellationToken token)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return await next().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Result.Error(ex, "Error occured, see Exception for more details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }

    private sealed class InterceptorTestsContext : IValidatableObject
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
