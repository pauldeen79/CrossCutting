namespace CrossCutting.ProcessingPipeline.Tests;

public class DecoratorTests
{
    [Fact]
    public async Task Can_Decorate_PipelineComponents()
    {
        // Arrange
        var sut = new Pipeline<DecoratorTestsContext>([Decorate(new MyComponent())]);
        var context = new DecoratorTestsContext();

        // Act
        var result = await sut.ProcessAsync(context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        context.ToString().ShouldBe(@"--- Start ---
MyComponent called
--- End ---
");

    }

    private static LoggerDecorator Decorate(IPipelineComponent<DecoratorTestsContext> decoratee)
        => new LoggerDecorator(new ValidationDecorator<DecoratorTestsContext>(new ExceptionDecorator<DecoratorTestsContext>(decoratee)));

    private sealed class MyComponent : IPipelineComponent<DecoratorTestsContext>
    {
        public Task<Result> ProcessAsync(PipelineContext<DecoratorTestsContext> context, CancellationToken token)
            => Task.Run(() =>
            {
                context.Request.AppendLine("MyComponent called");

                return Result.Success();
            }, token);
    }

    private sealed class LoggerDecorator : IPipelineComponent<DecoratorTestsContext>
    {
        private readonly IPipelineComponent<DecoratorTestsContext> _decoratee;

        public LoggerDecorator(IPipelineComponent<DecoratorTestsContext> decoratee)
        {
            _decoratee = decoratee;
        }

        public async Task<Result> ProcessAsync(PipelineContext<DecoratorTestsContext> context, CancellationToken token)
        {
            try
            {
                context.Request.AppendLine("--- Start ---");
                return await _decoratee.ProcessAsync(context, token).ConfigureAwait(false);
            }
            finally
            {
                context.Request.AppendLine("--- End ---");
            }
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
