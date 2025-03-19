namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public IntegrationTests()
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IFunction, MyBooleanFunction>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    [Fact]
    public void Can_Use_Operators_In_Expression_To_Produce_Boolean_Argument_Value()
    {
        // Arrange
        var parser = _scope.ServiceProvider.GetRequiredService<IFunctionParser>();
        var evaluator = _scope.ServiceProvider.GetRequiredService<IFunctionEvaluator>();
        var call = parser.Parse("MyBooleanFunction(1 == 1)", CultureInfo.InvariantCulture).GetValueOrThrow();

        // Act
        var evaluationResult = evaluator.Evaluate(call, new FunctionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture));

        // Assert
        evaluationResult.Status.ShouldBe(ResultStatus.Ok);
        evaluationResult.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Use_Binary_Operators_In_Expression_To_Produce_Boolean_Argument_Value()
    {
        // Arrange
        var parser = _scope.ServiceProvider.GetRequiredService<IFunctionParser>();
        var evaluator = _scope.ServiceProvider.GetRequiredService<IFunctionEvaluator>();
        var call = parser.Parse("MyBooleanFunction(true & true)", CultureInfo.InvariantCulture).GetValueOrThrow();

        // Act
        var evaluationResult = evaluator.Evaluate(call, new FunctionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture));

        // Assert
        evaluationResult.Status.ShouldBe(ResultStatus.Ok);
        evaluationResult.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Combine_Binary_Operators_With_Brackets_In_Mathemetic_Expression_For_Complex_Conditions()
    {
        // Arrange
        var input = "(true | false) & true";
        var sut = _scope.ServiceProvider.GetRequiredService<IMathematicExpressionEvaluator>();

        // Act
        var result = sut.Evaluate(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    [FunctionName("MyBooleanFunction")]
    [FunctionArgument("Expression", typeof(bool))]
    private sealed class MyBooleanFunction : ITypedFunction<bool>
    {
        public Result<object?> Evaluate(FunctionCallContext context)
            => EvaluateTyped(context).Transform<object?>(x => x);

        public Result<bool> EvaluateTyped(FunctionCallContext context)
            => new ResultDictionaryBuilder()
            .Add("Expression", () => context.GetArgumentBooleanValueResult(0, "Expression"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<bool>("Expression")));
    }
}
