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
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression("Hello world!")
            .Build();
        var request = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Validate(request);

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
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>()).Returns(x => Result.Success<object?>(x.ArgAt<string>(0)));
        var functionCall = new ToUpperCaseFunctionCallBuilder()
            .WithExpression("Hello world!")
            .Build();
        var request = new FunctionCallContext(functionCall, functionEvaluator, expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = sut.Evaluate(request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeOfType<string>();
        result.Value!.ToString().Should().Be("HELLO WORLD!");
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
        functionDescriptors.Single().Results.Should().HaveCount(3);
    }
}

[FunctionName(@"ToUpperCase")]
[Description("Converts the expression to upper case")]
[FunctionArgument("Expression", typeof(string), "String to get the upper case for", true)]
[FunctionArgument("Culture", typeof(CultureInfo), "Optional CultureInfo to use", false)]
[FunctionResult(ResultStatus.Ok, typeof(string), "The value of the expression converted to upper case", "This result will be returned when the expression is of type string")]
[FunctionResult(ResultStatus.Invalid, "Expression must be of type string")]
[FunctionResult(ResultStatus.Invalid, "CultureInfo must be of type CultureInfo")]
public class ToUpperCaseFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        //example for OnFailure that has a custom result, with an inner result that contains the details.
        //if you don't want an error message stating that this is the source, then simply remove the OnFailure line.
        return new ResultDictionaryBuilder()
            .Add("Expression", () => context.GetArgumentValueResult<string>(0, "Expression"))
            .Add("Culture", () => context.GetArgumentValueResult<CultureInfo?>(1, "Culture", null))
            .Build()
            .OnFailure(error => Result.Error<object?>([error], "ToUpperCase evaluation failed, see inner results for details"))
            .OnSuccess(results =>
                Result.Success<object?>(results["Culture"].GetValue() is null
                    ? results["Expression"].CastValueAs<string>().ToUpperInvariant()
                    : results["Expression"].CastValueAs<string>().ToUpper(results["Culture"].CastValueAs<CultureInfo>())));
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
