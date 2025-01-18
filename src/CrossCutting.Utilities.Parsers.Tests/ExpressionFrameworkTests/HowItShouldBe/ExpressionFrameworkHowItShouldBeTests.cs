namespace CrossCutting.Utilities.Parsers.Tests.ExpressionFrameworkTests.HowItShouldBe;

public class ExpressionFrameworkHowItShouldBeTests
{
    [Fact]
    public void Can_Validate_ToUpperCaseExpression()
    {
        // Arrange
        var sut = new ToUpperCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression("Hello world!")
            .Build();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Validate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Can_Evaluate_ToUpperCaseExpression()
    {
        // Arrange
        var sut = new ToUpperCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression("Hello world!")
            .Build();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Evaluate(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.ToString().Should().Be("HELLO WORLD!");
    }

    [Fact]
    public void Can_Evaluate_ToUpperCaseExpression_Typed()
    {
        // Arrange
        var sut = new ToUpperCaseFunction();
        var functionEvaluator = Substitute.For<IFunctionEvaluator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression(Result.Success("Hello world!"))
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .Build();
        var context = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.EvaluateTyped(context);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.Should().Be("HELLO WORLD!");
    }

    [Fact]
    public void Can_Get_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = new FunctionDescriptorProvider(new FunctionDescriptorMapper(), [new ToUpperCaseFunction()]);

        // Act
        var functionDescriptors = functionDescriptorProvider.GetAll();

        // Assert
        functionDescriptors.Should().ContainSingle();
        functionDescriptors.Single().Arguments.Should().HaveCount(2);
        functionDescriptors.Single().Results.Should().ContainSingle();
    }
}

// *** Generated code
[FunctionName(@"ToUpperCase")]
[Description("Converts the expression to upper case")]
[FunctionArgument("Expression", typeof(string), "String to get the upper case for", true)]
[FunctionArgument("Culture", typeof(CultureInfo), "Optional CultureInfo to use", false)]
[FunctionResult(ResultStatus.Ok, typeof(string), "The value of the expression converted to upper case", "This result will be returned when the expression is of type string")]
// No need to tell what is returned on invalid types of arguments, the framework can do this for you
public class ToUpperCaseFunction : ITypedFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        return EvaluateTyped(context).Transform<object?>(value => value);
    }

    public Result<string> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            //note that you can use both GetArgumentValueResult<string> or GetArgumentStringValueResult.
            .Add("Expression", () => context.GetArgumentStringValueResult(0, "Expression"))
            .Add("Culture", () => context.GetArgumentValueResult<CultureInfo>(1, "Culture", default))
            .Build()
            //example for OnFailure that has a custom result, with an inner result that contains the details.
            //if you don't want an error message stating that this is the source, then simply remove the OnFailure line.
            .OnFailure(OnFailure)
            .OnSuccess(OnSuccess(context));
    }

    // Strongly-typed access to arguments
    private static string Expression(Dictionary<string, Result> resultDictionary) => resultDictionary.GetValue<string>("Expression");
    private static CultureInfo? CultureInfo(Dictionary<string, Result> resultsDictionary, CultureInfo? defaultValue) => resultsDictionary.TryGetValue("Culture", defaultValue);

    // *** Scaffold code, by default throw a NotImplementedException.
    private static Func<Dictionary<string, Result>, Result<string>> OnSuccess(FunctionCallContext context)
    {
        return results => Result.Success(Expression(results).ToUpper(CultureInfo(results, context.FormatProvider.ToCultureInfo())));
    }

    private static Result OnFailure(Result error)
    {
        // If you want to return the error unchanged, just use return error to let it bubble up (default behavior?)
        return error;
        ///example for custom error: return Result.Error([error], "ToUpperCase evaluation failed, see inner results for details");
    }

    public Result Validate(FunctionCallContext context)
    {
        // No additional validation needed (default behavior)
        return Result.Success();
    }
}

public class ToUpperCaseFunctionCallBuilder : IBuilder<FunctionCall>
{
    public FunctionCallArgumentBuilder<string> Expression { get; set; }
    public FunctionCallArgumentBuilder<CultureInfo?> CultureInfo { get; set; }

    public ToUpperCaseFunctionCallBuilder()
    {
        // Same functionality as in ClassFramework.Pipelines: When it's a non-nullable string, then assign String.Empty. (and also, initialize collections and required builder-typed properties to new instances)
        Expression = new ConstantArgumentBuilder<string>().WithValue(string.Empty);
        CultureInfo = new ConstantArgumentBuilder<CultureInfo?>();
    }

    public ToUpperCaseFunctionCallBuilder WithExpression(FunctionCallArgumentBuilder<string> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Expression = expression;
        return this;
    }

    public ToUpperCaseFunctionCallBuilder WithCultureInfo(FunctionCallArgumentBuilder<CultureInfo?> cultureInfo)
    {
        ArgumentNullException.ThrowIfNull(cultureInfo);
        CultureInfo = cultureInfo;
        return this;
    }

    public FunctionCall Build()
    {
        return new FunctionCallBuilder()
            .WithName(@"ToUpperCase")
            .AddArguments(Expression, CultureInfo)
            .Build();
    }
}
