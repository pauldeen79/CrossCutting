namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    [Fact]
    public void Can_Evaluate_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = sut.Evaluate(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = sut.Evaluate(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyFunction(context)";

        // Act
        var result = sut.EvaluateTyped<string>(CreateContext(expression, context: "hello world"));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("HELLO WORLD");
    }

    public IntegrationTests()
    {
        Provider = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddSingleton<IFunction, MyFunction>()
            .BuildServiceProvider();
        Scope = Provider.CreateScope();
    }

    private ServiceProvider Provider { get; set; }
    private IServiceScope Scope { get; set; }

    private IExpressionEvaluator CreateSut() => Scope.ServiceProvider.GetRequiredService<IExpressionEvaluator>();

    public void Dispose()
    {
        Scope.Dispose();
        Provider.Dispose();
    }

    [FunctionName("MyFunction")]
    [FunctionArgument("Input", typeof(string))]
    private sealed class MyFunction : IFunction<string>
    {
        public Result<object?> Evaluate(FunctionCallContext context)
            => EvaluateTyped(context).Transform<object?>(x => x);

        public Result<string> EvaluateTyped(FunctionCallContext context)
            => new ResultDictionaryBuilder<string>()
                .Add("Input", () => context.Context.Evaluate(context.FunctionCall.Arguments.First()))
                .Build()
                .OnSuccess(results => Result.Success(results["Input"].Value.ToStringWithDefault().ToUpperInvariant()));
    }
}
