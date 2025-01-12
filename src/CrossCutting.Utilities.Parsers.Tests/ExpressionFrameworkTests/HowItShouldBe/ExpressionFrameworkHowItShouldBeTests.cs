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
            .WithExpression("Hello world!")
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
        var functionDescriptorProvider = new FunctionDescriptorProvider([new ToUpperCaseFunction()]);

        // Act
        var functionDescriptors = functionDescriptorProvider.GetAll();

        // Assert
        functionDescriptors.Should().ContainSingle();
        functionDescriptors.Single().Arguments.Should().HaveCount(2);
        functionDescriptors.Single().Results.Should().ContainSingle();
    }
}

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
        return EvaluateTyped(context).TryCast<object?>();
    }

    public Result<string> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add("Expression", () => context.GetArgumentValueResult<string>(0, "Expression"))
            .Add("Culture", () => context.GetArgumentValueResult<CultureInfo?>(1, "Culture", null))
            .Build()
            //example for OnFailure that has a custom result, with an inner result that contains the details.
            //if you don't want an error message stating that this is the source, then simply remove the OnFailure line.
            .OnFailure(error => Result.Error<object?>([error], "ToUpperCase evaluation failed, see inner results for details"))
            .OnSuccess(results =>
                Result.Success(results["Culture"].GetValue() is null
                    ? results.GetValue<string>("Expression").ToUpperInvariant()
                    : results.GetValue<string>("Expression").ToUpper(results.GetValue<CultureInfo>("Culture"))));
    }

    public Result Validate(FunctionCallContext context)
    {
        // No additional validation needed
        return Result.Success();
    }
}

public class ToUpperCaseFunctionCallBuilder : IBuilder<FunctionCall>
{
    public string Expression { get; set; }
    public CultureInfo? CultureInfo { get; set; }

    public ToUpperCaseFunctionCallBuilder()
    {
        // Same functionality as in ClassFramework.Pipelines: When it's a non-nullable string, then assign String.Empty. (and also, initialize collections and required builder-typed properties to new instances)
        Expression = string.Empty;
    }

    public ToUpperCaseFunctionCallBuilder WithExpression(string expression)
    {
        Expression = expression;
        return this;
    }

    public FunctionCall Build()
    {
        return new FunctionCallBuilder()
            .WithName(@"ToUpperCase")
            .AddArguments(
                new DelegateArgumentBuilder().WithDelegate(() => Expression),
                new DelegateArgumentBuilder().WithDelegate(() => CultureInfo)
            )
            .Build();
    }
}
