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

    [Fact]
    public void Can_Evaluate_Generic_Function_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "MyGenericFunction<System.String>()";

        // Act
        var result = sut.Evaluate(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    public IntegrationTests()
    {
        Provider = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddSingleton<IFunction, MyFunction>()
            .AddSingleton<IGenericFunction, MyGenericFunction>()
            .BuildServiceProvider();
    }

    private ServiceProvider Provider { get; set; }

    private IExpressionEvaluator CreateSut() => Provider.GetRequiredService<IExpressionEvaluator>();

    public void Dispose() => Provider.Dispose();

    [FunctionName("MyFunction")]
    [FunctionArgument("Input", typeof(string))]
    private sealed class MyFunction : IFunction<string>
    {
        public Result<object?> Evaluate(FunctionCallContext context)
            => EvaluateTyped(context).Transform<object?>(x => x);

        public Result<string> EvaluateTyped(FunctionCallContext context)
            => new ResultDictionaryBuilder()
                .Add("Input", () => context.GetArgumentValueResult<string>(0, "Input"))
                .Build()
                .OnSuccess(results => Result.Success(results.GetValue<string>("Input").ToUpperInvariant()));
    }

    [FunctionName("MyGenericFunction")]
    [FunctionTypeArgument("T", "Type argument to use")]
    private sealed class MyGenericFunction : IGenericFunction
    {
        public Result<object?> EvaluateGeneric<T>(FunctionCallContext context)
            => Result.Success<object?>(typeof(T));
    }
}
